using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using GemBox.Spreadsheet;
using Tesseract;
using System.Runtime.InteropServices;


namespace Citiroc_serialTest
{

    public partial class Form1 : Form
    {

        #region Paramètres Utilisateur
        public const float delta_asic_x = (float)-14.6, delta_asic_y = (float)-14.75;   //Delta de distance entre 2 ASICs (mm)
        public const float delta_plateau = (float)-134.5; //Distance verticale entre la première case de deux plateaux
        public int n_ligne = 5, n_colonne = 4, n_plateau = 1, n_plateau_bad = 1;  //Nombre d'ASICs par ligne et colonne, vu d'en face du robot
        public float x_asic0 = -102, y_asic0 = -160;  //Position de l'ASIC en haut à droite du premier plateau
        public float x_asic0_bad = -102, y_asic0_bad = -160;  //Position de l'ASIC en haut à droite du premier plateau
        public const float z_pos_asic = -82;        //Hauteur des ASICs sur leur plateau
        public const float x_tb = -220, y_tb = -311, z_tb = -83;     //Paramètres de position de la test board
        public const float offset_camera_x = (float)22, offset_camera_y = (float)40, offset_camera_z = (float)16; //Offset caméra. relatif à un ASIC
        public const float offset_actuator_y = -50; //Offset verrin (Coordonnées absolues)

        public int x0 = 1200, y0 = 1420;  //Position 0 du crop de la photo de l'ASIC
        public int longueur = 460, largeur = 220;   //Dimensions du crop
        public int contr = 0;   //Augmentation du contraste à appliquer (il est appliqué 2x)
        public int longueur_size = 150, largeur_size = 100;     //Dimensions de l'image redimensionné pour être donné à l'OCR
        public int seuil = 0;   //Plus le seuil est petit, plus le gris a besoin d'être sombre pour ne pas être passé en blanc

        public const int timeout_time = 18000; //Temps laissé à une commande avant de provoquer un timeout si un acknowledgement n'est pas reçu (en ms)

        /*******************************************************************************************************************
         * - Déclaration des plateaux, l'allocation se fait dans la fonction d'appuie du bouton de Test.                   *
         * - La fonction importante est State Machine et les fonctions appelées par cette dernière.                        *
         * - De base, elle utilisera un plateau nommé plateau_bon dans lequel il ira chercher les ASICs et les placer      *
         *   si le test est bon, et un nommé plateau_mauvais dans lequel il ira placer les mauvais ASICs                   *
         * - Si plusieurs plateaux, il faudra adapter. Possibilité de faire un tableau tridimensionel pour les bons.       *
         * - Cela nécessitera d'ajouter un index à i_boucle et j_boucle                                                    *
         ******************************************************************************************************************/
        ASIC[,,] plateau_bon, plateau_mauvais;
        #endregion

        #region Déclaration Variables Globales et Threads
        /*******************DECLARATION DES THREADS*************************/
        Thread mainflow;        //"Main" du programme, commande la machine à état.
        Thread datareceived;    //Thread de réception et de traitement des données
        Thread cmdexecution;    //Thread transmettant les commandes aux Arduinos


        /********************VARIABLES DE LA CAMERA********************/
        static int MAXBUFF = 524288; //512kb
        private List<byte> buffer = new List<byte>(MAXBUFF);
        private byte[] camdata = new byte[MAXBUFF];
        bool is_header = false;
        bool is_camdone = false;
        bool is_connected = false;
        bool is_capturing = false;
        bool is_2640 = false;
        bool is_5642 = false;
        bool is_streaming = false;
        int length = 0, n = 0;
        Stopwatch sw = new Stopwatch();


        /*****************VARIABLE DE MOUVEMENT********************/
        bool is_paused = false;
        bool is_pos_query = false;  //Quand une demande de position est émise
        bool is_homing = false;  //Quand la machine réalise un homing
        bool is_idle = true;     //Quand la machine est immobile
        bool first_homing = true;    //Quand on est au premier homing
        bool sometime = false;   //Quand au moins commande a été lancé, déclenche la prise de position
        bool cmd_done = false;   //Quand toutes les commandes dans le buffer ont été transmises
        bool cycle_over = false;     //Quand un cycle de test est fini, passe à l'ASIC suivant
        bool exec_done = true;   //Quand une réponse "ok" est reçu de la machine, ou pour commencer l'execution
        bool started = false;    //Quand on appuie sur le bouton de lancement du test
        int i_boucle = 0, j_boucle = 0, n_boucle = 0;     //Index de l'ASIC en traitement
        bool is_right_asic = true;
        bool is_restarted = false;
        string savepath = "";    //Chemin de sauvegarde de l'image traitée


        /**************DECLARATION DES STRINGS******************/
        String serialno = ""; //Numéro de série de l'ASIC en traitement
        String x_pos = "", y_pos = "", z_pos = "", state = ""; //Positions carthésiennes pour affichage
        String buffer_cmd = "", buffer_machine = "";  //Buffer des zones de textes
        String pos = "";     //Contient le message issu d'une demande de position
        String path = "";    //Dossier des photos
        String rxdata = "";  //Buffer de récéption des données
        String position = "";


        /*********BUFFER DE COMMANDES GCODE************/
        static List<String> gcode_cmd = new List<String>();

        private static Form_SerialTest testForm = new Form_SerialTest();

        ASIC test_board; //Déclaration des la test board

        // Chemin du Répertoire de Tesseract
        static String solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        static String tesseractPath = solutionDirectory + @"\tesseract-master.1153";
        #endregion

        public Form1()
        {
            InitializeComponent();
            getAvailablePorts();

            path = Application.StartupPath;

            mainflow = new Thread(new ThreadStart(MainFlow));
            mainflow.IsBackground = true;
            mainflow.Start();

            datareceived = new Thread(new ThreadStart(DataReceived));
            datareceived.IsBackground = true;
            datareceived.Start();

            cmdexecution = new Thread(new ThreadStart(CmdExecution));
            cmdexecution.IsBackground = true;
            cmdexecution.Start();

            testForm.Date = DateTime.Now.ToString("hh_mm_ss");

            test_board = new ASIC(x_tb, y_tb, z_tb); //Déclaration des la test board

            numericUpDown2.Value = x0;
            numericUpDown3.Value = y0;
            numericUpDown4.Value = longueur;
            numericUpDown5.Value = largeur;
            numericUpDown1.Value = contr;
            numericUpDown6.Value = longueur_size;
            numericUpDown7.Value = largeur_size;
            numericUpDown8.Value = seuil;
            tb_x_bon.Text = x_asic0.ToString();
            tb_y_bon.Text = y_asic0.ToString();
            tb_x_bad.Text = x_asic0_bad.ToString();
            tb_y_bad.Text = x_asic0_bad.ToString() ;
        }


        /**************************THREADS************************/

        //Thread gérant l'envoi des commandes aux Arduino
        public void CmdExecution()
        {
            while (true)
            {
                while (serialPort1.IsOpen || serialPort2.IsOpen || serialPort3.IsOpen) //Mettre && à terme
                {
                    if (sw.IsRunning && sw.ElapsedMilliseconds > timeout_time)
                    {
                        MessageBox.Show("TIMEOUT : " + gcode_cmd[n-1] + "\r\n ARRET DU PROGRAMME.");
                        is_paused = true;
                        sw.Reset();
                        init_cnc();
                        is_capturing = false;
                        end_thread();
                    }

                    if (serialPort1.IsOpen && !is_homing && sometime) //Prise récurrente de position, débute au lancement d'une commande. (Facultatif)
                    {
                        try
                        {
                            is_pos_query = true;
                            serialPort1.WriteLine("?");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        Thread.Sleep(50);
                    }

                    if (cmd_done && !is_paused /*&& is_right_asic*/) //Quand un "ok" de la machien est reçu, lance la commande suivante
                    {
                        lock (gcode_cmd)    //Evite que gcode-cmd soit modifié par plusieurs threads en même temps.
                        {
                            if (n < gcode_cmd.Count) //Tant qu'il reste des commandes à envoyer
                            {
                                if (gcode_cmd[n].StartsWith("CAM")) //Si c'est une prise de photo
                                {
                                    buffer_cmd = gcode_cmd[n];

                                    #region Temporisation tant que le bras est en mouvement
                                    Thread.Sleep(100);
                                    try
                                    {
                                        is_pos_query = true;
                                        serialPort1.WriteLine("?");
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                    Thread.Sleep(50);
                                    while (!is_idle) //Tant que la tête n'est pas immobile
                                    {
                                        try
                                        {
                                            is_pos_query = true;
                                            serialPort1.WriteLine("?");
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                        Thread.Sleep(50);
                                    }
                                    this.Invoke(new EventHandler(print_cmd));
                                    #endregion

                                    is_paused = true; //Pause le temps de prendre le photo et de l'analyser
                                    is_capturing = true;    //Pour la récpetion des données
                                    try
                                    {
                                        sw.Start();
                                        serialPort2.WriteLine("CAM\n");
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }

                                else if (gcode_cmd[n].StartsWith("PUMP") || gcode_cmd[n].StartsWith("ACTUATOR"))   //Si on agit sur la pompe à air ou le verin
                                {
                                    buffer_cmd = gcode_cmd[n];
                                    this.Invoke(new EventHandler(print_cmd));
                                    try
                                    {
                                        sw.Start();
                                        serialPort3.WriteLine(gcode_cmd[n]);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }

                                else if (gcode_cmd[n].StartsWith("?"))      //Si on envoie une demande de position
                                {
                                    try
                                    {
                                        is_pos_query = true;
                                        serialPort1.WriteLine("?");
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }

                                else if (gcode_cmd[n].StartsWith("$h"))     //Si on envoie une demande de homing
                                {
                                    if (!first_homing)  //Si ce n'est pas l'homing de début de programme (Sans cette condition le programme bug)
                                    {
                                        #region Temporisation tant que le bras est en mouvement
                                        try
                                        {
                                            is_pos_query = true;
                                            serialPort1.WriteLine("?");
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                        Thread.Sleep(50);
                                        while (!is_idle) //Tant que la tête n'est pas immobile
                                        {
                                            try
                                            {
                                                is_pos_query = true;
                                                serialPort1.WriteLine("?");
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message);
                                            }
                                            Thread.Sleep(50);
                                        }
                                        #endregion
                                    }

                                    is_homing = true;   //On entre dans un homing, empêche la demande de position
                                    first_homing = false;
                                    buffer_cmd = gcode_cmd[n];
                                    this.Invoke(new EventHandler(print_cmd));
                                    try
                                    {
                                        sw.Start();
                                        serialPort1.WriteLine(gcode_cmd[n]);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }

                                else          //Si c'est tout autre commande, probablement un mouvement
                                {
                                    #region Temporisation tant que le bras est en mouvement
                                    try
                                    {
                                        is_pos_query = true;
                                        serialPort1.WriteLine("?");
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                    Thread.Sleep(50);
                                    while (!is_idle) //Tant que la tête n'est pas immobile
                                    {
                                        try
                                        {
                                            is_pos_query = true;
                                            serialPort1.WriteLine("?");
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                        Thread.Sleep(50);
                                    }
                                    #endregion
                                    // Attendre la fin du mouvement précédent avant d'en envoyer un nouveau empêche le programme de s'emballer.
                                    // L'utilisateur est donc mieux en contrôle du déroulement, en particulier pour ajouter des états ou des tests

                                    buffer_cmd = gcode_cmd[n];
                                    this.Invoke(new EventHandler(print_cmd));
                                    try
                                    {
                                        sw.Start();
                                        serialPort1.WriteLine(gcode_cmd[n]);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }

                                n++;
                                cmd_done = false;   //Repasse cmd_done à false. Un "ok" reçu d'une Arduino repassera la variable à true, déclenchant une nouvelle transmission de commande
                            }
                            else    // Si on est arrivé au bout du buffer de commandes à envoyer
                            {
                                this.Invoke(new EventHandler(end_test));
                                cmd_done = false;
                            }
                        }
                    }
                    Thread.Sleep(50);
                }
            }
        }

        //Fonction invoquée quand on arrive à la fin du buffer de commande
        public void end_test(object sender, EventArgs e)
        {
            if (is_restarted)
            {
                is_paused = true;
                i_boucle = 0;
                j_boucle = 0;
                n_boucle = 0;
                cycle_over = true;
                gcode_cmd.Clear();
                init_cnc();
                is_paused = false;
                exec_done = true;
                is_restarted = false;
            }
            else
            {
                n = 0;
                gcode_cmd.Clear();
                exec_done = true;
            }
        }

        //Thread contrôlant la machine à état
        public void MainFlow()
        {
            while (true)
            {
                while (started)	//Débute quand on appuie sur le bouton de lancement
                {
                    for (n_boucle = 0; n_boucle < n_plateau; n_boucle++)
                        for (i_boucle = 0; i_boucle < n_ligne; i_boucle++)  //Parcours du plateau
                            for (j_boucle = 0; j_boucle < n_colonne; j_boucle++)
                            {
                                while (!cycle_over) //Tant que le cycle de test n'est pas fini
                                {
                                    if (!is_paused)
                                        this.Invoke(new EventHandler(State_Machine));   //Rentre dans la machine à état
                                    Thread.Sleep(10);
                                }
                                cycle_over = false;
                                gcode_cmd.Add("G00X-10Y-10Z-10"); //Rapproche le bras de sa position de homing, pour éviter de perdre du temps
                                gcode_cmd.Add(Homing());
                                testForm.Date = DateTime.Now.ToString("hh_mm_ss");
                            }
                    started = false;
                }
                Thread.Sleep(10);
            }
        }

        //Thread contrôlant la récéption des données en provenance des Arduinos
        private void DataReceived()
        {
            while (true)
            {
                /***********************Gère la récéption des données**********************/
                if (is_capturing)
                { //Si la réception fait suite à une prise de photo, récupère les données et les met dans "buffer" pour être traité dans plus tard dans le thread
                    #region Collecte des données photo
                    try
                    {
                        if ((serialPort2.IsOpen == true) && (serialPort2.BytesToRead > 0))
                        {
                            int n = serialPort2.BytesToRead;
                            byte[] buf = new byte[n];
                            serialPort2.Read(buf, 0, n);
                            buffer.AddRange(buf);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    #endregion
                }
                else if (is_pos_query) //Si la réception fait suite à une prise de position
                {// Le texte reçu a comme format : < Idle | MPos:-5.000,-5.000,-5.000 | Bf:14,127 | FS:0,0 >
                    #region Traitement et affichage des coordonnées
                    Thread.Sleep(20); //Permet d'attendre que tout soit reçu
                    int cut1 = 0, cut2 = 0;
                    try
                    {
                        pos = serialPort1.ReadExisting();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    position = pos;
                    is_idle = (pos.Contains("Idle") ? true : false); //Test si le robot est au repos


                    /************SEPARATION DE L'ETAT ET DES X, Y ET Z POUR AFFICHAGE**************/
                    pos = pos.Remove(0, 1); //On enlève "< "
                    cut1 = pos.IndexOf(('|')); //On récupère l'index du caractère de séparation 
                    cut2 = pos.Length - cut1; //On détermine combien de caractère sont à enlever, ici tout en partant de la virgule
                    try
                    {
                        state = pos.Remove(cut1, cut2);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        buffer_machine = "Echec de la prise de position";
                    }

                    pos = pos.Remove(0, pos.IndexOf(':') + 1); //On enlève le début, jusqu'aux positions
                    cut1 = pos.IndexOf((',')); //On récupère l'index du caractère de séparation 
                    cut2 = pos.Length - cut1; //On détermine combien de caractère sont à enlever, ici tout en partant de la virgule
                    try
                    {
                        x_pos = pos.Remove(cut1, cut2);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        buffer_machine = "Echec de la prise de position";
                    }

                    pos = pos.Remove(0, pos.IndexOf(',') + 1);
                    cut1 = pos.IndexOf((','));
                    cut2 = pos.Length - cut1;
                    try
                    {
                        y_pos = pos.Remove(cut1, cut2);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        buffer_machine = "Echec de la prise de position";
                    }

                    pos = pos.Remove(0, pos.IndexOf(',') + 1);
                    cut1 = pos.IndexOf(('|'));
                    cut2 = pos.Length - cut1;
                    try
                    {
                        z_pos = pos.Remove(cut1, cut2);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        buffer_machine = "Echec de la prise de position";
                    }
                    /****************************************************************/

                    is_pos_query = false;
                    try
                    {
                        this.Invoke(new EventHandler(displaypos));
                    }
                    catch (ObjectDisposedException)
                    {
                        datareceived.Abort();
                    }
                    #endregion
                    }
                else //Sinon, met les données dans rxdata
                {
                    #region réception et affichage des autres données
                    if ((serialPort1.IsOpen == true) && (serialPort1.BytesToRead > 0))
                    {
                        try
                        {
                            rxdata += serialPort1.ReadExisting();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        this.Invoke(new EventHandler(displaytext));
                    }
                    if ((serialPort2.IsOpen == true) && (serialPort2.BytesToRead > 0))
                    {
                        try
                        {
                            rxdata += serialPort2.ReadExisting();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        this.Invoke(new EventHandler(displaytext));
                    }
                    if ((serialPort3.IsOpen == true) && (serialPort3.BytesToRead > 0))
                    {
                        try
                        {
                            rxdata += serialPort3.ReadExisting();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        this.Invoke(new EventHandler(displaytext));
                    }
                    #endregion
                }

                /**************TRAITEMENT DES DONNÉES RECUES VENANT DE LA CAMERA*******************/
                if (is_connected && is_capturing)
                {

                    #region Reconstruction de l'image
                    while ((buffer.Count > 2))
                    {
                        if ((is_header == false) && (buffer[0] == 0xFF) && (buffer[1] == 0xD8))
                        {
                            is_header = true;
                            break;
                        }
                        else if ((is_header == true) && (is_camdone == false))
                        {
                            int position = 0;
                            while (((buffer[position] != 0xFF) || (buffer[position + 1] != 0xD9)))
                            {
                                position += 1;
                                if ((position + 2) >= buffer.Count)
                                {
                                    break;
                                }
                            }
                            if ((buffer[position] == 0xFF) && (buffer[position + 1] == 0xD9))
                            {
                                is_camdone = true;
                                length = position + 2;
                                Array.Copy(buffer.ToArray(), camdata, position + 2);
                                buffer.RemoveRange(0, position + 2);
                            }
                            break;
                        }
                        else if (is_header == false)
                        {
                            byte[] temp = new byte[5];
                            temp[0] = buffer[0];
                            if (buffer[0] == 0x0D)
                                buffer.RemoveAt(0);
                            if ((buffer[0] == 0x4F) && (buffer[1] == 0x56) && (is_2640 == false) && (is_5642 == false) && (buffer.Count > 5))
                            {

                                if ((buffer[2] == 0x32) && (buffer[3] == 0x36) && (buffer[4] == 0x34) && (buffer[5] == 0x30))
                                {
                                    is_2640 = true;
                                }
                                else if ((buffer[2] == 0x35) && (buffer[3] == 0x36) && (buffer[4] == 0x34) && (buffer[5] == 0x32))
                                {
                                    is_5642 = true;
                                }
                            }
                            buffer.RemoveAt(0);
                        }
                        else
                            break;
                    }
                    #endregion

                    if (is_camdone == true)
                    {
                        sw.Reset();
                        if (is_streaming) //Si c'est une prise en continue, ne passe pas is_capturing à false pour permettre la récupération des donneés en continu
                        {
                            DispPictureBox1.Image = BytesToBitmap(camdata);
                            is_header = false;
                            is_camdone = false;
                        }
                        else if (checkBox1.Checked)
                        { //Ne fais pas l'OCR et affiche la photo original, si la checkBox "Sans OCR" est checked
                            DispPictureBox1.Image = BytesToBitmap(camdata);
                            is_header = false;
                            is_camdone = false;
                            is_capturing = false;
                            BytesToFile(camdata);
                            if (!but_test.Text.Equals("Lancement du Test"))
                                is_paused = false;
                            cmd_done = true;
                        }
                        else
                        { //2 fonctions OCR, une pour le plateau standard, et une pour la test-board car l'environnement, et donc l'éclairage est différent
                            BytesToFile(camdata);
                            this.Invoke(new EventHandler(OCR));
                        }

                        Array.Clear(camdata, 0, camdata.Length);
                    }
                }
                Thread.Sleep(10);
            }
        }

        /*******************AUTRES FONCTIONS**********************/

        #region Fonctions des plateaux
        //Alloue un plateau, en donnant les coordonnées des ASICs à partir de l'ASICs de départ (en haut à droite)
        public ASIC[,,] grid_creation(float start_x, float start_y, int n_grid)
        {
            ASIC[,,] ret = new ASIC[n_plateau,n_ligne, n_colonne];
            for (int n = 0; n < n_grid; n++)
                for (int i = 0; i < n_ligne; i++)
                    for (int j = 0; j < n_colonne; j++)
                        ret[n, i, j] = new ASIC(start_x + i * delta_asic_x, start_y + j * delta_asic_y + n * delta_plateau, z_pos_asic, false);
            return ret;
        }

        //Fonction retournant la première case libre d'un plateau
        public ASIC getFirstFreeAsic(ASIC[,,] plateau, int n_grid)
        {
            for (int n = 0; n < n_grid; n++)
                for (int i = 0; i < n_ligne; i++)
                    for (int j = 0; j < n_colonne; j++)
                        if (!plateau[n, i, j].isOccupied)
                        return plateau[n, i, j];

            return null;
        }
        #endregion

        #region Fonctions Caméra
        //Fonction permettant l'affichage de l'image sur l'interface
        private Image BytesToBitmap(byte[] bu)
        {
            MemoryStream stream = null;
            byte[] image_data = new byte[length];
            Bitmap resize_img = new Bitmap(DispPictureBox1.Width, DispPictureBox1.Height);
            Graphics graphic = Graphics.FromImage(resize_img);

            Array.Copy(bu, image_data, length);

            try
            {
                stream = new MemoryStream(image_data);

                Bitmap result = new Bitmap(stream);

                graphic.InterpolationMode = InterpolationMode.High;

                graphic.DrawImage(result, new Rectangle(0, 0, DispPictureBox1.Width, DispPictureBox1.Height));
                graphic.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                stream.Close();
            }
            return new Bitmap((Image)resize_img);

        }

        //Fonction permettant de sauvegarder l'image dans un fichier disque
        private void BytesToFile(byte[] bu)
        {
            byte[] image_data = new byte[length];
            String cam = "";
            if (is_2640 == true)
                cam = "OV2640";
            else if (is_5642 == true)
                cam = "OV5642";
            savepath = path + "\\" + cam + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";

            FileStream fs = File.Create(savepath);

            //image_data = buffer.ToArray();
            Array.Copy(bu, image_data, length);
            fs.Write(image_data, 0, image_data.Length);
            fs.Close();

        }
        #endregion

        #region Fonctions Intéraction avec Interface
        private void CapButton_Click(object sender, EventArgs e)
        {
            if (CapButton.Text.Equals("Capture")) //Prise d'une seule photo
            {
                try
                {
                    if (serialPort2.IsOpen)
                    {
                        serialPort2.WriteLine("CAM\n");
                        tb_machine.AppendText("Capturing.\n");
                        is_capturing = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR");
                }
            }
            else if (CapButton.Text.Equals("Start")) //Prise de photo en continue
            {
                is_streaming = true;
                CapButton.Text = "Stop";
                cb_continue.Enabled = false;
                try
                {
                    if (serialPort2.IsOpen)
                    {
                        serialPort2.WriteLine("STREAM\n");
                        tb_machine.AppendText("Streaming.\n");
                        is_capturing = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR");
                }
            }
            else if (CapButton.Text.Equals("Stop")) //Arrête la prise en continue
            {
                try
                {
                    if (serialPort2.IsOpen)
                    {
                        serialPort2.WriteLine("STOP\n");
                        tb_machine.AppendText("Stop streaming.\n");
                        is_capturing = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR");
                }
                is_streaming = false;
                cb_continue.Enabled = true;
                cb_continue.Checked = false;
                CapButton.Text = "Start";
            }

        }

        //Ouverture des ports séries
        private void but_port_Click(object sender, EventArgs e)
        {
            try
            {
                if (cb_camera_port.Text == "" || cb_cnc_port.Text == "" || cb_arduino_port.Text == "")
                {
                    tb_machine.Text = "Veuillez paramètrer les ports séries";
                }
                else
                {
                    serialPort1.PortName = cb_cnc_port.Text;
                    serialPort1.BaudRate = 115200;
                    serialPort2.PortName = cb_camera_port.Text;
                    serialPort2.BaudRate = 921600;
                    serialPort3.PortName = cb_arduino_port.Text;
                    serialPort3.BaudRate = 9600;
                    serialPort1.Open();
                    serialPort2.Open();
                    serialPort3.Open();

                    but_port.Enabled = false;
                    but_cnc.Enabled = false;
                    but_arduino.Enabled = false;
                    but_test.Enabled = true;
                    but_close.Enabled = true;
                    tb_gcode_in.Enabled = true;
                    tb_gcode_out.Text = "";
                    is_connected = true;
                    tb_machine.AppendText("\rPort Opened.\n");
                }
            }
            catch (UnauthorizedAccessException)
            {
                tb_machine.Text = "Accès à un port non authorisé";
            }
        }

        //Fonction de fermeture des ports séries
        private void but_close_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            serialPort2.Close();
            but_port.Enabled = true;
            but_close.Enabled = false;
            tb_gcode_in.Enabled = false;
            but_cnc.Enabled = true;
            but_arduino.Enabled = true;
            is_header = false;
            is_camdone = false;
            is_connected = false;
            tb_machine.Text = "Port Fermé.\n";
            gcode_cmd.Clear();
            tb_machine.AppendText("Port Closed.\n");
        }

        //CheckBox pour gérer la capture ou la prise continue
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_continue.Checked)
            {
                CapButton.Text = "Start";
                but_test.Enabled = false;
                checkBox1.Enabled = false;
            }
            else
            {
                CapButton.Text = "Capture";
                but_test.Enabled = true;
                checkBox1.Enabled = true;
            }
        }

        private void But_reset_Click(object sender, EventArgs e)
        {
            is_restarted = true;
        }

        //Fonction permettant de lancer et mettre en pause le test
        private void but_test_Click(object sender, EventArgs e)
        {
            sometime = true; //Déclenche la prise de postion
            but_reset.Enabled = true;
            if (but_test.Text == "Lancement du Test")
            {
                tb_machine.Text = "";
                but_test.Text = "Pause du Test";
                is_paused = false;
                CapButton.Enabled = false;
                cb_continue.Enabled = false;
            }
            else
            {	//Pour mettre en pause
                is_paused = true;
                tb_machine.Text = "Programme en pause";
                but_test.Text = "Lancement du Test";
                CapButton.Enabled = true;
                cb_continue.Enabled = true;
            }

            if (!started) //Si on appuie sur le bouton pour la première fois
            {
                testForm.Show();
                testForm.ConnectUSB();
                testForm.Hide();
                plateau_bon = grid_creation(x_asic0, y_asic0, n_plateau);  //Alloue les plateaux
                plateau_mauvais = grid_creation(x_asic0_bad, y_asic0_bad, n_plateau_bad);
                init_cnc(); //Initialise
                gcode_cmd.Add(Homing());	//Homing de lancement
                started = true;	//Déclenche le lancement de la machine à état
            }
        }


        //Permet d'envoyer une seule commande en la tapant dans l'espace de texte G-Code de gauche, et en appuyant sur la touche Entrée
        private void Tb_gcode_in_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmd_done = false;
                buffer_cmd = tb_gcode_in.Text;
                if (tb_gcode_in.Text.StartsWith("CAM"))
                {
                    is_paused = true; //Pause le temps de prendre le photo et de l'analyser
                    is_capturing = true;
                    serialPort2.WriteLine("CAM\n");
                }
                else if (tb_gcode_in.Text.StartsWith("PUMP") || tb_gcode_in.Text.StartsWith("ACTUATOR"))
                {
                    tb_gcode_out.Text += tb_gcode_in.Text;
                    serialPort3.WriteLine(tb_gcode_in.Text);
                }
                else if (tb_gcode_in.Text.StartsWith("?"))
                {
                    is_pos_query = true;
                    serialPort1.WriteLine(tb_gcode_in.Text);
                }
                else
                {
                    serialPort1.WriteLine(tb_gcode_in.Text);
                }
                but_test.Text = "Lancement du Test";
            }
        }

        //Permet de connecter chaque port individuellement
        private void Button2_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = cb_cnc_port.Text;
            serialPort1.BaudRate = 115200;
            try
            {
                serialPort1.Open();
            }
            catch (UnauthorizedAccessException)
            {
                tb_machine.Text = "Accès à un port non authorisé";
            }
            but_port.Enabled = false;
            but_test.Enabled = true;
            but_close.Enabled = true;
            tb_gcode_in.Enabled = true;
            tb_gcode_out.Text = "";
            is_connected = true;
            tb_machine.AppendText("\rPort CNC Opened.\n");
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            serialPort2.PortName = cb_camera_port.Text;
            serialPort2.BaudRate = 921600;
            try
            {
                serialPort2.Open();
            }
            catch (UnauthorizedAccessException)
            {
                tb_machine.Text = "Accès à un port non authorisé";
            }
            but_port.Enabled = false;
            but_test.Enabled = true;
            but_close.Enabled = true;
            tb_gcode_in.Enabled = true;
            tb_gcode_out.Text = "";
            is_connected = true;
            tb_machine.AppendText("\rPort Camera Opened.\n");
        }
        private void Button2_Click_1(object sender, EventArgs e)
        {
            serialPort3.PortName = cb_arduino_port.Text;
            //serialPort3.BaudRate = Convert.ToInt32(cb_arduino_br.Text);
            serialPort3.BaudRate = 9600;
            try
            {
                serialPort3.Open();
            }
            catch (UnauthorizedAccessException)
            {
                tb_machine.Text = "Accès à un port non authorisé";
            }
            but_port.Enabled = false;
            but_test.Enabled = true;
            but_close.Enabled = true;
            tb_gcode_in.Enabled = true;
            tb_gcode_out.Text = "";
            is_connected = true;
            tb_machine.AppendText("\rPort Arduino Opened.\n");
        }

        //Applique le changement de paramètres
        private void But_set_Click(object sender, EventArgs e)
        {
            x0 = (int)numericUpDown2.Value;
            y0 = (int)numericUpDown3.Value;
            longueur = (int)numericUpDown4.Value;
            largeur = (int)numericUpDown5.Value;
            contr = (int)numericUpDown1.Value;
            longueur_size = (int)numericUpDown6.Value;
            largeur_size = (int)numericUpDown7.Value;
            seuil = (int)numericUpDown8.Value;
        }

        //Permet de faire l'analyse OCR sur une photo à partir d'un bouton
        private void But_analysis_Click(object sender, EventArgs e)
        {
            Image im = null, fin = null;
            Bitmap f = null, ftemp = null;

            try
            {
                im = Image.FromFile(savepath);              //Récupération de l'image
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Fichier inexistant");
            }

            //Recupération des valeurs pour le crop de l'image

            x0 = (int)numericUpDown2.Value;
            y0 = (int)numericUpDown3.Value;
            longueur = (int)numericUpDown4.Value;
            largeur = (int)numericUpDown5.Value;
            contr = (int)numericUpDown1.Value;
            longueur_size = (int)numericUpDown6.Value;
            largeur_size = (int)numericUpDown7.Value;
            seuil = (int)numericUpDown8.Value;

            //Crop de l'image
            Rectangle r = new Rectangle(x0, y0, longueur, largeur);                       //Rectangle pour le crop de l'image
            fin = cropImage(im, r);                                           //Crop de l'image  
            try
            {
                f = new Bitmap(fin);
            }
            catch (NullReferenceException)
            {
                tb_machine.Text = "Dimensions Incorrects";
                return;
            }

            //Double boucle pour l'inversion des couleurs
            for (int y = 0; (y <= (f.Height - 1)); y++)
            {
                for (int x = 0; (x <= (f.Width - 1)); x++)
                {
                    Color inv = f.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    f.SetPixel(x, y, inv);
                }
            }

            fin = f;
            ToGrayScale((Bitmap)fin);   //Rend l'image uniquement en nuance de gris
            SetContrast((Bitmap)fin, contr);           //Changement de contrastetr);

            if (blancNoir.Checked) //Si on active l'affichage uniquement ne noir et blanc
            {
                ftemp = new Bitmap(fin);
                for (int y = 0; (y <= (ftemp.Height - 1)); y++)
                {
                    for (int x = 0; (x <= (ftemp.Width - 1)); x++)
                    {
                        Color inv = ftemp.GetPixel(x, y);
                        if (!(inv.R < seuil && inv.G < seuil && inv.B < seuil)) //Transforme tous les pixels dont la nuance est en dessous du seuil en pixel blanc
                        {
                            inv = Color.FromArgb(255, 255, 255, 255);
                            ftemp.SetPixel(x, y, inv);
                        }
                    }
                }

                fin = ftemp;
            }

            String dest = solutionDirectory + @"\samplesCrop\\" + testForm.Date + ".jpg";  //Chemin pour la sauvegarde de l'image
            saveJpeg(dest, (Bitmap)fin, 32);                                  //Sauvegarde de l'image
            String temp = "";                                                                  //Ecriture du résultat*/
                                                                                               //Redimensionnement pour affichage
            Size s = new Size(640, 520);
            Image ff = resizeImage(fin, s);
            DispPictureBox1.Image = ff;
            // Redimensionnement pour l'OCR, qui marche mieux avec des dimensions moindres
            s = new Size(longueur_size, largeur_size);
            Image f_im = resizeImage(fin, s);
            saveJpeg(dest, (Bitmap)f_im, 32);

            // Application de l'OCR
            var imageFile = File.ReadAllBytes(dest);                            //Lecture des bits de l'image
            String textRes = ParseText(tesseractPath, imageFile, "eng", "fra");                 //Lecture du texte contenu dans l'image
            textBox3.Text = textRes;


            for (int i = 0; i < textRes.Length; i++)
            {
                if (textRes[i] >= '0' && textRes[i] <= '9') //Ne garde que les chiffres
                    temp += textRes[i];
            }
            textBox3.Text = temp;

            dest = solutionDirectory + @"\samplesCrop\\" + testForm.Date + "+" + temp + ".jpg";
            saveJpeg(dest, (Bitmap)f_im, 32);
        }


        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            tb_gcode_out.Text = "";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            testForm.cl();
            //Quand on ferme l'application, termine les threads
            end_thread();

        }




        #endregion

        #region Fonctions Machine à Etats
        void init_cnc()
        {
            nextState = States.Positionning;
            inProcess = Processus.Retrieving;
        }

        //Machine a état principale
        void State_Machine(object sender, EventArgs e)
        {
            ASIC asic = plateau_bon[n_boucle, i_boucle, j_boucle];

            if (exec_done && !is_paused && testForm.IsFinished/* && is_right_asic*/) //Attend la fin de l'exécution des commandes si on en a déclencher une
            {
                currentState = nextState;

                switch (currentState)
                {
                    case States.Positionning:
                        Positionning(asic, plateau_bon, plateau_mauvais);
                        nextState = States.Lowering;
                        break;

                    case States.Lowering:
                        Lowering(asic, plateau_bon, plateau_mauvais);
                        //nextState = States.Uppering;
                        nextState = States.Releasing;
                        //nextState = States.Picture_Taking;
                        break;

                    case States.Releasing:
                        Releasing(asic);
                        nextState = States.Picture_Taking;
                        break;

                    case States.Picture_Taking:
                        Picture_Taking(asic);
                        //nextState = States.Analysing;
                        //nextState = States.Testing;
                        nextState = States.Grabbing;
                        break;

                    case States.Analysing:
                        Analysing(asic);
                        //nextState = States.Testing;
                        //nextState = States.Uppering;
                        break;

                    case States.Holding:
                        Holding();
                        //nextState = States.Testing;
                        nextState = States.Positionning;
                        break;

                    case States.Testing:
                        Testing(asic);
                        //nextState = States.Grabbing;
                        nextState = States.Uppering;
                        break;

                    case States.Grabbing:
                        Grabbing(asic);
                        nextState = States.Uppering;
                        break;

                    case States.Uppering:
                        Uppering(asic);
                        nextState = States.Positionning;
                        break;
                }
            }
        }

        //Fonction créant le G-Code de positionnement
        void Positionning(ASIC asic, ASIC[,,] plateau_bon, ASIC[,,] plateau_mauvais)
        {
            lock (gcode_cmd)
            {
                switch (inProcess)
                {
                    case Processus.Retrieving:	//Quand on se positionne pour aller chercher l'ASIC à tester
                        gcode_cmd.Add(asic.getPositionningCmd());
                        break;

                    case Processus.Testing:	//Quand on se positionne au dessus de la test board
                        gcode_cmd.Add(test_board.getPositionningCmd());
                        break;

                    case Processus.Sorting:	//Quand on se positionne pour trier l'ASIC
                        if (asic.isGood)
                        {
                            gcode_cmd.Add(getFirstFreeAsic(plateau_bon, n_plateau).getPositionningCmd());
                        }
                        else
                        {
                            gcode_cmd.Add(getFirstFreeAsic(plateau_bon, n_plateau_bad).getPositionningCmd());
                        }
                        break;
                }
            }
            this.tb_gcode_in.AppendText("\rPOSITIONNING ( " + toString(inProcess) + " ) : " + gcode_cmd[gcode_cmd.Count - 1] + "\r\n");
        }

        //Fonction créant le G-Code pour baisser le bras
        void Lowering(ASIC asic, ASIC[,,] plateau_bon, ASIC[,,] plateau_mauvais)
        {
            lock (gcode_cmd)
            {
                switch (inProcess)
                {
                    case Processus.Retrieving:
                        gcode_cmd.Add(asic.getLoweringCmd());
                        break;

                    case Processus.Testing:
                        gcode_cmd.Add(test_board.getLoweringCmd());
                        break;

                    case Processus.Sorting:
                        if (asic.isGood)
                        {
                            gcode_cmd.Add(getFirstFreeAsic(plateau_bon, n_plateau).getLoweringCmd());
                        }
                        else
                        {
                            gcode_cmd.Add(getFirstFreeAsic(plateau_mauvais, n_plateau_bad).getLoweringCmd());
                        }
                        break;
                }
            }
            this.tb_gcode_in.AppendText("\r LOWERING ( " + toString(inProcess) + " ) : " + gcode_cmd[gcode_cmd.Count - 1] + "\r\n");
        }

        //Fonction créant le G-code pour lever le bras
        void Uppering(ASIC asic)
        {
            testForm.Hide();
            lock (gcode_cmd)
            {
                switch (inProcess)
                {
                    case Processus.Retrieving:
                        gcode_cmd.Add(asic.getUpperingCmd());
                        gcode_cmd.Add("PUMP ?");
                        //inProcess = Processus.Testing;
                        //Passage au processus Testing dans displayText, uniquement quand la pompe tient l'ASIC
                        break;

                    case Processus.Testing:
                        gcode_cmd.Add(asic.getUpperingCmd());
                        gcode_cmd.Add("PUMP ?");
                        //inProcess = Processus.Sorting;
                        //Passage au processus Sorting dans displayText, uniquement quand la pompe tient l'ASIC
                        break;

                    case Processus.Sorting:
                        gcode_cmd.Add(asic.getUpperingCmd());
                        inProcess = Processus.Retrieving;
                        cycle_over = true; //Le cycle est terminé, on change d'ASIC
                        break;
                }
            }
            this.tb_gcode_in.AppendText("\r UPPERING ( " + toString(inProcess) + " ) : " + gcode_cmd[gcode_cmd.Count - 1] + "\r\n");
            exec_done = false;
            cmd_done = true;
        }

        //Fonction créant le G-Code pour amèner la caméra au point voulu au dessus de l'ASIC et le prendre en photo
        void Picture_Taking(ASIC asic)
        {
            serialno = "";
            //lock (gcode_cmd)
            {
                switch (inProcess)
                {
                    case Processus.Retrieving:
                        gcode_cmd.Add(getCameraOffsetCmd(asic));
                        gcode_cmd.Add("CAM");
                        break;

                    case Processus.Testing:
                        gcode_cmd.Add(getCameraOffsetCmd(test_board));
                        gcode_cmd.Add("CAM");
                        break;

                    case Processus.Sorting:
                        gcode_cmd.Add(getCameraOffsetCmd(asic));
                        gcode_cmd.Add("CAM");
                        break;
                }
            }
            this.tb_gcode_in.AppendText("\r IMAGE_TAKING ( " + toString(inProcess) + " ) : " + gcode_cmd[gcode_cmd.Count - 1] + "\r\n");
            exec_done = false;
            cmd_done = true; //Exécute l'enchainement Levage/Positionnement/Abaissement
        }

        //Fonction s'assurant que l'on est bien en possession du même ASIC pendant les étapes du déplacement
        private void Analysing(ASIC asic)
        {
            switch (inProcess)
            {
                case Processus.Retrieving:	//Attribue le numéro de l'ASIC en test 
                    plateau_bon[n_boucle, i_boucle, j_boucle].Serial = serialno;
                    is_right_asic = true;
                    break;

                case Processus.Testing:	//Si le numéro que l'on observe est différent de celui de l'ASIC que l'on testait, arrête tout
                    is_right_asic = asic.Serial.Equals(serialno);
                    break;

                case Processus.Sorting:
                    is_right_asic = asic.Serial.Equals(serialno);
                    break;
            }
            this.tb_gcode_in.AppendText("\r ANALYSIS ( " + toString(inProcess) + " ) : " + is_right_asic + "\n");
        }

        private void Holding()
        {
            switch (inProcess)
            {
                case Processus.Retrieving:
                    break;

                case Processus.Testing:
                    gcode_cmd.Add(getActuatorOffsetCmd());
                    //gcode_cmd.Add("ACTUATOR"); AJOUTER ACTION VERRIN
                    this.tb_gcode_in.AppendText("\r HOLDING ( " + toString(inProcess) + " ) : " + gcode_cmd[gcode_cmd.Count - 1] + "\r\n");
                    break;

                case Processus.Sorting:
                    break;
            }
        }

        private void Testing(ASIC asic)
        {
            switch (inProcess)
            {
                case Processus.Retrieving:
                    break;

                case Processus.Testing:
                    this.tb_gcode_in.AppendText("\r TESTING ( " + toString(inProcess) + "  )\n");
                    testForm.Show();
                    testForm.setTextBox(asic.Serial);
                    testForm.start();
                    break;
                case Processus.Sorting:
                    break;
            }

        }

        //Fonction créant le G-Code permettant de se rapprocher de l'emplacement et de lâcher l'ASIC
        private void Releasing(ASIC asic)
        {
            switch (inProcess)
            {
                case Processus.Retrieving:
                    break;

                case Processus.Testing:   //Se rapproche de l'emplacement et lâche l'ASIC
                                          //PUMP 0
                    gcode_cmd.Add(test_board.getTouchCmd());
                    gcode_cmd.Add("PUMP 0");
                    this.tb_gcode_in.AppendText("\r RELEASE ( " + toString(inProcess) + " ) : " + gcode_cmd[gcode_cmd.Count - 1] + "\r\n");
                    break;

                case Processus.Sorting:
                    //PUMP 0
                    gcode_cmd.Add(asic.getTouchCmd());
                    gcode_cmd.Add("PUMP 0");
                    plateau_bon[n_boucle, i_boucle, j_boucle].isOccupied = true;
                    this.tb_gcode_in.AppendText("\r RELEASE ( " + toString(inProcess) + " ) : " + gcode_cmd[gcode_cmd.Count - 1] + ", isOccupied[" + i_boucle + "," + j_boucle + "] = " + plateau_bon[n_boucle, i_boucle, j_boucle].isOccupied + "\r\n");
                    /*
                    if (asic.isGood)
                        getFirstFreeAsic(plateau_bon, n_plateau).isOccupied = true;
                    else
                        getFirstFreeAsic(plateau_mauvais, n_plateau_bad).isOccupied = true;*/
                    break;
            }
        }

        //Fonction créant le G-Code pour toucher l'ASIC et l'attraper
        private void Grabbing(ASIC asic)
        {
            switch (inProcess)
            {
                case Processus.Retrieving:    //Touche l'ASIC et l'attrape
                                              //PUMP 1
                    gcode_cmd.Add(asic.getPositionningCmd());
                    gcode_cmd.Add(asic.getTouchCmd());
                    gcode_cmd.Add("PUMP 1");
                    this.tb_gcode_in.AppendText("\r GRABBING ( " + toString(inProcess) + " ) : " + gcode_cmd[gcode_cmd.Count - 1] + "\r\n");
                    break;

                case Processus.Testing:
                    //PUMP 1
                    gcode_cmd.Add(test_board.getPositionningCmd());
                    gcode_cmd.Add(test_board.getTouchCmd());
                    gcode_cmd.Add("PUMP 1");
                    this.tb_gcode_in.AppendText("\r GRABBING ( " + toString(inProcess) + " ) : " + gcode_cmd[gcode_cmd.Count - 1] + "\r\n");
                    break;

                case Processus.Sorting:
                    break;
            }
        }


        #endregion

        #region Fonctions Affichage
        private void print_cmd(object sender, EventArgs e)
        {
            tb_cmd.Text = buffer_cmd;
            tb_gcode_out.Text += buffer_cmd;
            tb_gcode_out.Text += "\r\n";

        }




        //Fonction invoquée pour gérer parser et afficher les données renvoyées par les Arduinos
        private void displaytext(object sender, EventArgs e)
        {
            if (rxdata.Contains("\n"))
            {
                if ((rxdata.Contains("ok") || rxdata.Contains("OK")) && sw.IsRunning) //ACK de la commande passé reçu, on reset le chronomètre des timeout
                    sw.Reset();

                if (rxdata.StartsWith("ok"))	// Envoyé par la CNC pour indiquer que la commande a été executée
                {
                    tb_gcode_out.Text += "ACK OK";
                    tb_gcode_out.Text += "\r\n";
                    cmd_done = true;	//On déclenche l'envoi de la commande suivante
                    is_homing = false;
                }
                else if (rxdata.StartsWith("OK PUMP 1"))
                {
                    tb_gcode_out.Text += "ACK PUMP 1";
                    tb_gcode_out.Text += "\r\n";
                    tb_airpump.BackColor = System.Drawing.Color.Green;
                    tb_machine.Text += "Activation de la pompe\r\n";
                    cmd_done = true;
                }
                else if (rxdata.StartsWith("OK PUMP 0"))
                {
                    tb_gcode_out.Text += "ACK PUMP 0";
                    tb_gcode_out.Text += "\r\n";
                    tb_airpump.BackColor = System.Drawing.Color.Red;
                    tb_machine.Text += "Désactivation de la pompe\r\n";
                    cmd_done = true;
                }
                else if (rxdata.StartsWith("OK ACTUATOR 0"))
                {
                    tb_gcode_out.Text += "ACK ACTUATOR 0";
                    tb_gcode_out.Text += "\r\n";
                    tb_airpump.BackColor = System.Drawing.Color.Red;
                    tb_machine.Text += "Désactivation du piston \r\n";
                    cmd_done = true;
                }
                else if (rxdata.StartsWith("OK ACTUATOR 1"))
                {
                    tb_gcode_out.Text += "ACK ACTUATOR 1";
                    tb_gcode_out.Text += "\r\n";
                    tb_airpump.BackColor = System.Drawing.Color.Red;
                    tb_machine.Text += "Activation du piston \r\n";
                    cmd_done = true;
                }
                else if (rxdata.StartsWith("KO ACTUATOR 1"))
                {
                    tb_gcode_out.Text += "ACK KO ACTUATOR 1";
                    tb_gcode_out.Text += "\r\n";
                    tb_airpump.BackColor = System.Drawing.Color.Red;
                    tb_machine.Text += "Echec de l'Activation du piston \r\n";
                    cmd_done = true;
                }
                else if (rxdata.StartsWith("OK ASIC"))
                {
                    tb_gcode_out.Text += "ACK ASIC";
                    tb_gcode_out.Text += "\r\n";
                    tb_machine.Text += "Ventouse en possession de l'ASIC \r\n";
                    tb_airpump.BackColor = System.Drawing.Color.Blue;
                    if (inProcess == Processus.Retrieving)
                    {
                        inProcess = Processus.Testing;
                        plateau_bon[n_boucle, i_boucle, j_boucle].isOccupied = false;
                    }
                    else if (inProcess == Processus.Testing)
                    {
                        inProcess = Processus.Sorting;
                    }
                    cmd_done = true;
                }
                else if (rxdata.StartsWith("OK !ASIC"))
                {
                    tb_gcode_out.Text += "ACK !ASIC";
                    tb_gcode_out.Text += "\r\n";
                    tb_machine.Text += "Ventouse pas en possession de l'ASIC \r\n";
                    tb_airpump.BackColor = System.Drawing.Color.Orange;
                    cmd_done = true;
                }
                else if (rxdata.StartsWith("TIMEOUT"))
                {
                    MessageBox.Show(rxdata + "ARRET DU PROGRAMME.");
                    //GO VERS SAFE POSITION?
                    is_paused = true;
                    serialPort3.Write("PUMP 0\n");
                    init_cnc();
                    end_thread();
                }
                else
                {
                    tb_gcode_out.Text += rxdata;
                }
                rxdata = "";
            }
        }


        private void But_grid_Click(object sender, EventArgs e)
        {
            n_plateau = (int) nb_plateau.Value;
            /*x_asic0 = (int) tb_x_bon.Text;
            y_asic0 = tb_y_bon.Text;
            x_asic0_bad = tb_x_bad.Text;
            y_asic0_bad = tb_y_bad.Text;*/
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ASIC a = new ASIC();
            a.Serial = "1234";
            Form_SerialTest test = new Form_SerialTest();
            test.Show();
            test.cl();
        }



        //Fonction invoquée pour afficher la position du bras
        private void displaypos(object sender, EventArgs e)
        {
            tb_xpos.Text = x_pos;
            tb_ypos.Text = y_pos;
            tb_zpos.Text = z_pos;
            tb_etat.Text = state;
        }


        public void print_machine(object sender, EventArgs e)
        {
            tb_machine.Text = buffer_machine;
        }
        #endregion

        #region Fonctions Diverses 

        void end_thread()
        {
            mainflow.Abort();
            datareceived.Abort();
            cmdexecution.Abort();
        }
        String toString(Processus process)
        {
            return process.ToString("g");
        }

        String toString(States state)
        {
            return state.ToString("g");
        }

        String getCameraOffsetCmd(ASIC asic)
        {
            float tempx = asic.X + offset_camera_x;
            float tempy = asic.Y + offset_camera_y;
            float tempz = asic.Z + offset_camera_z;
            return "G00X" + tempx.ToString() + "Y" + tempy.ToString() + "Z" + tempz.ToString();
        }

        String getReverseCameraOffsetCmd(ASIC asic)
        {
            float tempx = asic.X - offset_camera_x;
            float tempy = asic.Y - offset_camera_y;
            return "G00X" + tempx.ToString() + "Y" + tempy.ToString();
        }

        String getActuatorOffsetCmd()
        {
            return "G00Y" + offset_actuator_y.ToString();
        }

        //Fonction renvoyant le g-code pour faire un Homing (prise de 0)
        String Homing()
        {
            return "$h";
        }


        void getAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            cb_cnc_port.Items.AddRange(ports);
            cb_camera_port.Items.AddRange(ports);
            cb_arduino_port.Items.AddRange(ports);
        }
        #endregion

        #region Fonctions OCR
        //Fonction pour l'OCR. Les seules fonctions destinées à être modifiées sont les fonctions OCR et OCR_test
        private static string ParseText(string tesseractPath, byte[] imageFile, params string[] lang)
        {
            string output = string.Empty;
            var tempOutputFile = Path.GetTempPath() + Guid.NewGuid();
            var tempImageFile = Path.GetTempFileName();

            try
            {
                File.WriteAllBytes(tempImageFile, imageFile);

                ProcessStartInfo info = new ProcessStartInfo();
                info.WorkingDirectory = tesseractPath;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.UseShellExecute = false;
                info.FileName = "cmd.exe";
                info.Arguments =
                    "/c tesseract.exe " +
                    // Image file.
                    tempImageFile + " " +
                    // Output file (tesseract add '.txt' at the end)
                    tempOutputFile +
                    // Languages.
                    " -l " + string.Join("+", lang);

                // Start tesseract.
                Process process = Process.Start(info);
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    // Exit code: success.
                    output = File.ReadAllText(tempOutputFile + ".txt");
                }
                else
                {
                    throw new Exception("Error. Tesseract stopped with an error code = " + process.ExitCode);
                }
            }
            finally
            {
                File.Delete(tempImageFile);
                File.Delete(tempOutputFile + ".txt");
            }
            return output;
        }

        public void ToGrayScale(Bitmap Bmp)
        {
            int rgb;
            Color c;

            for (int y = 0; y < Bmp.Height; y++)
                for (int x = 0; x < Bmp.Width; x++)
                {
                    c = Bmp.GetPixel(x, y);
                    rgb = (int)Math.Round(.299 * c.R + .587 * c.G + .114 * c.B);
                    Bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        private static void SetContrast(Bitmap bmp, int threshold)
        {
            var lockedBitmap = new LockBitmap(bmp);
            lockedBitmap.LockBits();

            var contrast = Math.Pow((100.0 + threshold) / 100.0, 2);

            for (int y = 0; y < lockedBitmap.Height; y++)
            {
                for (int x = 0; x < lockedBitmap.Width; x++)
                {
                    var oldColor = lockedBitmap.GetPixel(x, y);
                    var red = ((((oldColor.R / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    var green = ((((oldColor.G / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    var blue = ((((oldColor.B / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    if (red > 255) red = 255;
                    if (red < 0) red = 0;
                    if (green > 255) green = 255;
                    if (green < 0) green = 0;
                    if (blue > 255) blue = 255;
                    if (blue < 0) blue = 0;

                    var newColor = Color.FromArgb(oldColor.A, (int)red, (int)green, (int)blue);
                    lockedBitmap.SetPixel(x, y, newColor);
                }
            }
            lockedBitmap.UnlockBits();
        }

        private void saveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = this.getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = null;
            try
            {
                bmpCrop = bmpImage.Clone(cropArea,
                bmpImage.PixelFormat);
            }
            catch (System.OutOfMemoryException)
            {
                MessageBox.Show("Dimensions incorrects");
            }
            return (Image)(bmpCrop);
        }

        #endregion
        //Fonction OCR appliqué pour les plateaux
        void OCR(object sender, EventArgs e)
        {
            #region Définitions de variables
            Image im = null, fin = null, f_im = null, ff = null;
            String temp = "", ret = "";
            List<String> past_ocr = new List<string>();
            Bitmap f = null, ftemp = null;
            textBox3.Text = "";

            int contrast_threashold = 50, white_threshold = 15;
            #endregion

            #region récupération de l'image et pré-traitement
            try
            {
                im = Image.FromFile(savepath);              //Récupération de l'image
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Fichier inexistant");
            }

            //Crop de l'image
            Rectangle r = new Rectangle(x0, y0, longueur, largeur);                       //Rectangle pour le crop de l'image
            fin = cropImage(im, r);                                           //Crop de l'image 

            try
            {
                f = new Bitmap(fin); //Si le traitement d'image fait dépasser les dimensions, interrompt l'OCR
            }
            catch (Exception ex)
            {
                tb_machine.Text = "Erreur de dimensions";
                is_header = false;
                is_camdone = false;
                is_capturing = false;
                is_paused = false;
                cmd_done = true;
                return;
            }

            //Double boucle pour l'inversion des couleurs
            for (int y = 0; (y <= (f.Height - 1)); y++)
                for (int x = 0; (x <= (f.Width - 1)); x++)
                {
                    Color inv = f.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    f.SetPixel(x, y, inv);
                }

            ToGrayScale((Bitmap)f);

            String dest = solutionDirectory + @"\samplesCrop\\" + testForm.Date + "BeforeRead.jpg";   //Chemin pour la sauvegarde de l'image
            #endregion

            if (inProcess == Processus.Testing)
            {
                contr = 25;  //Contraste de départ
                contrast_threashold = 32; //Contraste max
                white_threshold = 18;   //Quand la valeur RGB du gris est >18, le pixel passe en blanc
                //Plus cette valeur est faible, plus le gris a besoin d'être foncé pour ne pas passer en pixel blanc
            }
            else
            {
                contr = 32;
                contrast_threashold = 52;
                white_threshold = 15;
            }

            while (true)
            {
                temp = "";
                fin = f;
                contr = (inProcess == Processus.Testing ? contr + 1 : contr + 2); //+1 si testing, +2 sinon


                if (contr > contrast_threashold)
                    break;
                SetContrast((Bitmap)fin, contr);           //Changement de contraste


                ftemp = new Bitmap(fin);
                for (int y = 0; (y <= (ftemp.Height - 1)); y++) //Passage en noir et blanc
                {
                    for (int x = 0; (x <= (ftemp.Width - 1)); x++) //Si le gris n'est pas assez prononcé, le pixel devient blanc
                    {
                        Color inv = ftemp.GetPixel(x, y);
                        if (!(inv.R < white_threshold && inv.G < white_threshold && inv.B < white_threshold))
                        {
                            inv = Color.FromArgb(255, 255, 255, 255);
                            ftemp.SetPixel(x, y, inv);
                        }
                    }
                }
                fin = ftemp;

                //Redimensionnement pour affichage
                Size s = new Size(640, 520);
                ff = resizeImage(fin, s);
                DispPictureBox1.Image = ff;


                // Redimensionnement pour l'OCR, qui marche mieux avec des dimensions moindres
                s = new Size(longueur_size, largeur_size);
                f_im = resizeImage(fin, s);
                saveJpeg(dest, (Bitmap)f_im, 32);

                // Application de l'OCR
                var imageFile = File.ReadAllBytes(dest);                            //Lecture des bits de l'image
                String textRes = ParseText(tesseractPath, imageFile, "eng", "fra");                 //Lecture du texte contenu dans l'image

                if (started)
                {
                    #region Traitement du texte obtenu
                    for (int i = 0; i < textRes.Length; i++) //Parcours la String rendu par l'OCR
                    {
                        if (textRes[i] >= '0' && textRes[i] <= '9') //Si c'est un chiffre, l'ajoute au résultat
                        {

                            if (temp.Length == 0 && textRes[i] == '4')
                            {
                                temp += '1';
                            }
                            if (temp.Length == 1 && textRes[i] == '1')
                            {
                                temp += '7';
                            }
                            else
                            {
                                temp += textRes[i];
                            }

                        }
                        if (temp.Length == 5)
                            break;
                    }
                    textBox3.Text += temp + '/'; //Affiche le nombre trouvé

                    if (temp.Length == 5) //Si le nombre est bien à 5 chiffres
                    {
                        if (past_ocr.Count == 0)
                            past_ocr.Add(temp);
                        else
                        {
                            for (int i = 0; i < past_ocr.Count; i++) //Regarde si le nombre a déjà été trouvé, sinon l'ajoute à past_ocr
                            {
                                if (temp.Equals(past_ocr[i])) //Si c'est le cas, le numéro de série prend ce nombre
                                {
                                    ret = temp;
                                    goto Suite; //Sors de la boucle for et while
                                }
                                else
                                    past_ocr.Add(temp);
                            }
                        }
                    }
                    #endregion
                }
            }

        Suite:
            if (ret.Length == 0 && past_ocr.Count > 0) //Si on a pas trouvé 2 fois le même mais que des nombres ont été trouvé, le numéro de série prend le dernier nombre trouvé
                ret = past_ocr[past_ocr.Count - 1];

            textBox3.Text += "/   " + ret + "   /";

            dest = solutionDirectory + @"\samplesCrop\\" + testForm.Date + "+" + temp + ".jpg";
            saveJpeg(dest, (Bitmap)f_im, 32);
            is_header = false;
            is_camdone = false;
            is_capturing = false;
            if (!but_test.Text.Equals("Lancement du Test"))
                is_paused = false;
            cmd_done = true;
        }

    }
}