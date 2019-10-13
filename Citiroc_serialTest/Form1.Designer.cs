using System;
using System.Collections.Generic;

namespace Citiroc_serialTest
{
    partial class Form1
    {

        enum Processus { Retrieving, Testing, Sorting };
        enum States { Positionning, Lowering, Holding, Uppering, Execution, Analysing, Releasing, Picture_Taking, Grabbing, Idle, Testing };
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nb_plateau = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_y_bad = new System.Windows.Forms.TextBox();
            this.tb_x_bad = new System.Windows.Forms.TextBox();
            this.tb_y_bon = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_x_bon = new System.Windows.Forms.TextBox();
            this.but_grid = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_gcode_in = new System.Windows.Forms.TextBox();
            this.but_test = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tb_gcode_out = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tb_machine = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tb_cmd = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tb_etat = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tb_airpump = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_zpos = new System.Windows.Forms.TextBox();
            this.tb_ypos = new System.Windows.Forms.TextBox();
            this.tb_xpos = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.cb_arduino_port = new System.Windows.Forms.ComboBox();
            this.but_arduino = new System.Windows.Forms.Button();
            this.but_cnc = new System.Windows.Forms.Button();
            this.but_port = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cb_camera_port = new System.Windows.Forms.ComboBox();
            this.cb_cnc_port = new System.Windows.Forms.ComboBox();
            this.but_close = new System.Windows.Forms.Button();
            this.DispPictureBox1 = new System.Windows.Forms.PictureBox();
            this.CapButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown7 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.but_analysis = new System.Windows.Forms.Button();
            this.but_set = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cb_continue = new System.Windows.Forms.CheckBox();
            this.but_reset = new System.Windows.Forms.Button();
            this.serialPort3 = new System.IO.Ports.SerialPort(this.components);
            this.numericUpDown8 = new System.Windows.Forms.NumericUpDown();
            this.blancNoir = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nb_plateau)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DispPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nb_plateau);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tb_y_bad);
            this.groupBox1.Controls.Add(this.tb_x_bad);
            this.groupBox1.Controls.Add(this.tb_y_bon);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_x_bon);
            this.groupBox1.Controls.Add(this.but_grid);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(32, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 208);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration du plateau de test";
            // 
            // nb_plateau
            // 
            this.nb_plateau.BackColor = System.Drawing.SystemColors.Window;
            this.nb_plateau.ForeColor = System.Drawing.SystemColors.MenuText;
            this.nb_plateau.Location = new System.Drawing.Point(62, 46);
            this.nb_plateau.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nb_plateau.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nb_plateau.Name = "nb_plateau";
            this.nb_plateau.Size = new System.Drawing.Size(61, 20);
            this.nb_plateau.TabIndex = 30;
            this.nb_plateau.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(254, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Plateau Bon";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(245, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Plateau Mauvais";
            // 
            // tb_y_bad
            // 
            this.tb_y_bad.Location = new System.Drawing.Point(291, 95);
            this.tb_y_bad.Name = "tb_y_bad";
            this.tb_y_bad.Size = new System.Drawing.Size(27, 20);
            this.tb_y_bad.TabIndex = 14;
            this.tb_y_bad.Text = "21";
            // 
            // tb_x_bad
            // 
            this.tb_x_bad.Location = new System.Drawing.Point(259, 95);
            this.tb_x_bad.Name = "tb_x_bad";
            this.tb_x_bad.Size = new System.Drawing.Size(27, 20);
            this.tb_x_bad.TabIndex = 12;
            this.tb_x_bad.Text = "21";
            // 
            // tb_y_bon
            // 
            this.tb_y_bon.Location = new System.Drawing.Point(291, 58);
            this.tb_y_bon.Name = "tb_y_bon";
            this.tb_y_bon.Size = new System.Drawing.Size(27, 20);
            this.tb_y_bon.TabIndex = 11;
            this.tb_y_bon.Text = "21";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(214, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Coordonnées des 1e ASICs (x,y)";
            // 
            // tb_x_bon
            // 
            this.tb_x_bon.Location = new System.Drawing.Point(259, 58);
            this.tb_x_bon.Name = "tb_x_bon";
            this.tb_x_bon.Size = new System.Drawing.Size(27, 20);
            this.tb_x_bon.TabIndex = 9;
            this.tb_x_bon.Text = "21";
            // 
            // but_grid
            // 
            this.but_grid.Location = new System.Drawing.Point(44, 104);
            this.but_grid.Name = "but_grid";
            this.but_grid.Size = new System.Drawing.Size(152, 28);
            this.but_grid.TabIndex = 8;
            this.but_grid.Text = "Définir le plateau";
            this.but_grid.UseVisualStyleBackColor = true;
            this.but_grid.Click += new System.EventHandler(this.But_grid_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Nombre de plateaux (1 par défaut)";
            // 
            // tb_gcode_in
            // 
            this.tb_gcode_in.BackColor = System.Drawing.Color.White;
            this.tb_gcode_in.Enabled = false;
            this.tb_gcode_in.Location = new System.Drawing.Point(9, 20);
            this.tb_gcode_in.Multiline = true;
            this.tb_gcode_in.Name = "tb_gcode_in";
            this.tb_gcode_in.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_gcode_in.Size = new System.Drawing.Size(166, 210);
            this.tb_gcode_in.TabIndex = 1;
            this.tb_gcode_in.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tb_gcode_in_KeyDown);
            // 
            // but_test
            // 
            this.but_test.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.but_test.ForeColor = System.Drawing.Color.Red;
            this.but_test.Location = new System.Drawing.Point(480, 177);
            this.but_test.Name = "but_test";
            this.but_test.Size = new System.Drawing.Size(226, 36);
            this.but_test.TabIndex = 3;
            this.but_test.Text = "Lancement du Test";
            this.but_test.UseVisualStyleBackColor = true;
            this.but_test.Click += new System.EventHandler(this.but_test_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox9);
            this.groupBox2.Controls.Add(this.groupBox8);
            this.groupBox2.Location = new System.Drawing.Point(32, 281);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 268);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "G-Code";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.tb_gcode_in);
            this.groupBox9.Location = new System.Drawing.Point(7, 19);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(180, 243);
            this.groupBox9.TabIndex = 57;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Debug MAE";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.tb_gcode_out);
            this.groupBox8.Location = new System.Drawing.Point(193, 18);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(180, 243);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Communication Arduinos";
            // 
            // tb_gcode_out
            // 
            this.tb_gcode_out.BackColor = System.Drawing.Color.White;
            this.tb_gcode_out.Location = new System.Drawing.Point(12, 19);
            this.tb_gcode_out.Multiline = true;
            this.tb_gcode_out.Name = "tb_gcode_out";
            this.tb_gcode_out.ReadOnly = true;
            this.tb_gcode_out.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_gcode_out.Size = new System.Drawing.Size(163, 218);
            this.tb_gcode_out.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(433, 281);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(351, 267);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Informations";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tb_machine);
            this.groupBox6.Location = new System.Drawing.Point(8, 19);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(337, 87);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Affichage Machine";
            // 
            // tb_machine
            // 
            this.tb_machine.BackColor = System.Drawing.Color.White;
            this.tb_machine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tb_machine.Location = new System.Drawing.Point(34, 19);
            this.tb_machine.Multiline = true;
            this.tb_machine.Name = "tb_machine";
            this.tb_machine.ReadOnly = true;
            this.tb_machine.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_machine.Size = new System.Drawing.Size(284, 62);
            this.tb_machine.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tb_cmd);
            this.groupBox5.Location = new System.Drawing.Point(8, 112);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(337, 50);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Commande en cours";
            // 
            // tb_cmd
            // 
            this.tb_cmd.BackColor = System.Drawing.Color.White;
            this.tb_cmd.Enabled = false;
            this.tb_cmd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.tb_cmd.Location = new System.Drawing.Point(34, 19);
            this.tb_cmd.Name = "tb_cmd";
            this.tb_cmd.Size = new System.Drawing.Size(284, 24);
            this.tb_cmd.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tb_etat);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.tb_airpump);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.tb_zpos);
            this.groupBox4.Controls.Add(this.tb_ypos);
            this.groupBox4.Controls.Add(this.tb_xpos);
            this.groupBox4.Location = new System.Drawing.Point(8, 168);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(337, 93);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Position du Bras";
            // 
            // tb_etat
            // 
            this.tb_etat.BackColor = System.Drawing.Color.White;
            this.tb_etat.Enabled = false;
            this.tb_etat.Location = new System.Drawing.Point(162, 43);
            this.tb_etat.Name = "tb_etat";
            this.tb_etat.Size = new System.Drawing.Size(50, 20);
            this.tb_etat.TabIndex = 10;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(115, 46);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(32, 13);
            this.label21.TabIndex = 9;
            this.label21.Text = "Etat :";
            // 
            // tb_airpump
            // 
            this.tb_airpump.BackColor = System.Drawing.Color.Red;
            this.tb_airpump.Enabled = false;
            this.tb_airpump.Location = new System.Drawing.Point(217, 63);
            this.tb_airpump.Name = "tb_airpump";
            this.tb_airpump.Size = new System.Drawing.Size(50, 20);
            this.tb_airpump.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(64, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(146, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Activation de la Pompe à Air :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(230, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Z :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(136, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Y :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "X :";
            // 
            // tb_zpos
            // 
            this.tb_zpos.BackColor = System.Drawing.Color.White;
            this.tb_zpos.Enabled = false;
            this.tb_zpos.Location = new System.Drawing.Point(256, 19);
            this.tb_zpos.Name = "tb_zpos";
            this.tb_zpos.Size = new System.Drawing.Size(50, 20);
            this.tb_zpos.TabIndex = 2;
            // 
            // tb_ypos
            // 
            this.tb_ypos.BackColor = System.Drawing.Color.White;
            this.tb_ypos.Enabled = false;
            this.tb_ypos.Location = new System.Drawing.Point(162, 19);
            this.tb_ypos.Name = "tb_ypos";
            this.tb_ypos.Size = new System.Drawing.Size(50, 20);
            this.tb_ypos.TabIndex = 1;
            // 
            // tb_xpos
            // 
            this.tb_xpos.BackColor = System.Drawing.Color.White;
            this.tb_xpos.Enabled = false;
            this.tb_xpos.Location = new System.Drawing.Point(67, 19);
            this.tb_xpos.Name = "tb_xpos";
            this.tb_xpos.Size = new System.Drawing.Size(54, 20);
            this.tb_xpos.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.button2);
            this.groupBox7.Controls.Add(this.label20);
            this.groupBox7.Controls.Add(this.cb_arduino_port);
            this.groupBox7.Controls.Add(this.but_arduino);
            this.groupBox7.Controls.Add(this.but_cnc);
            this.groupBox7.Controls.Add(this.but_port);
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Controls.Add(this.cb_camera_port);
            this.groupBox7.Controls.Add(this.cb_cnc_port);
            this.groupBox7.Location = new System.Drawing.Point(441, 30);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(343, 132);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Configuration Série";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(238, 58);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Définir Arduino";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click_1);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(240, 16);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(92, 13);
            this.label20.TabIndex = 13;
            this.label20.Text = "Port Série Arduino";
            // 
            // cb_arduino_port
            // 
            this.cb_arduino_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_arduino_port.FormattingEnabled = true;
            this.cb_arduino_port.Location = new System.Drawing.Point(238, 32);
            this.cb_arduino_port.Name = "cb_arduino_port";
            this.cb_arduino_port.Size = new System.Drawing.Size(90, 21);
            this.cb_arduino_port.TabIndex = 12;
            // 
            // but_arduino
            // 
            this.but_arduino.Location = new System.Drawing.Point(128, 58);
            this.but_arduino.Name = "but_arduino";
            this.but_arduino.Size = new System.Drawing.Size(89, 23);
            this.but_arduino.TabIndex = 11;
            this.but_arduino.Text = "Définir Caméra";
            this.but_arduino.UseVisualStyleBackColor = true;
            this.but_arduino.Click += new System.EventHandler(this.Button3_Click);
            // 
            // but_cnc
            // 
            this.but_cnc.Location = new System.Drawing.Point(13, 58);
            this.but_cnc.Name = "but_cnc";
            this.but_cnc.Size = new System.Drawing.Size(89, 23);
            this.but_cnc.TabIndex = 10;
            this.but_cnc.Text = "Définir CNC";
            this.but_cnc.UseVisualStyleBackColor = true;
            this.but_cnc.Click += new System.EventHandler(this.Button2_Click);
            // 
            // but_port
            // 
            this.but_port.Location = new System.Drawing.Point(97, 90);
            this.but_port.Name = "but_port";
            this.but_port.Size = new System.Drawing.Size(152, 28);
            this.but_port.TabIndex = 9;
            this.but_port.Text = "Définir les ports";
            this.but_port.UseVisualStyleBackColor = true;
            this.but_port.Click += new System.EventHandler(this.but_port_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(126, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Port Série Caméra";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Port Série CNC";
            // 
            // cb_camera_port
            // 
            this.cb_camera_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_camera_port.FormattingEnabled = true;
            this.cb_camera_port.Location = new System.Drawing.Point(128, 32);
            this.cb_camera_port.Name = "cb_camera_port";
            this.cb_camera_port.Size = new System.Drawing.Size(90, 21);
            this.cb_camera_port.TabIndex = 1;
            // 
            // cb_cnc_port
            // 
            this.cb_cnc_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_cnc_port.FormattingEnabled = true;
            this.cb_cnc_port.Location = new System.Drawing.Point(13, 32);
            this.cb_cnc_port.Name = "cb_cnc_port";
            this.cb_cnc_port.Size = new System.Drawing.Size(90, 21);
            this.cb_cnc_port.TabIndex = 0;
            // 
            // but_close
            // 
            this.but_close.Enabled = false;
            this.but_close.Location = new System.Drawing.Point(259, 247);
            this.but_close.Name = "but_close";
            this.but_close.Size = new System.Drawing.Size(152, 28);
            this.but_close.TabIndex = 9;
            this.but_close.Text = "Fermeture ports";
            this.but_close.UseVisualStyleBackColor = true;
            this.but_close.Click += new System.EventHandler(this.but_close_Click);
            // 
            // DispPictureBox1
            // 
            this.DispPictureBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.DispPictureBox1.Location = new System.Drawing.Point(791, 28);
            this.DispPictureBox1.Name = "DispPictureBox1";
            this.DispPictureBox1.Size = new System.Drawing.Size(640, 520);
            this.DispPictureBox1.TabIndex = 10;
            this.DispPictureBox1.TabStop = false;
            // 
            // CapButton
            // 
            this.CapButton.Location = new System.Drawing.Point(1099, 571);
            this.CapButton.Name = "CapButton";
            this.CapButton.Size = new System.Drawing.Size(144, 46);
            this.CapButton.TabIndex = 22;
            this.CapButton.Text = "Capture";
            this.CapButton.UseVisualStyleBackColor = true;
            this.CapButton.Click += new System.EventHandler(this.CapButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(57, 254);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(76, 17);
            this.checkBox1.TabIndex = 28;
            this.checkBox1.Text = "Sans OCR";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(4, 613);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 13);
            this.label13.TabIndex = 36;
            this.label13.Text = "longueur :";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(12, 636);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 13);
            this.label14.TabIndex = 35;
            this.label14.Text = "largeur :";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(33, 587);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(24, 13);
            this.label15.TabIndex = 34;
            this.label15.Text = "y0 :";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(34, 561);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(24, 13);
            this.label16.TabIndex = 33;
            this.label16.Text = "x0 :";
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDown5.ForeColor = System.Drawing.SystemColors.MenuText;
            this.numericUpDown5.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown5.Location = new System.Drawing.Point(64, 635);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown5.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown5.TabIndex = 32;
            this.numericUpDown5.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDown4.ForeColor = System.Drawing.SystemColors.MenuText;
            this.numericUpDown4.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown4.Location = new System.Drawing.Point(64, 610);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown4.TabIndex = 31;
            this.numericUpDown4.Value = new decimal(new int[] {
            450,
            0,
            0,
            0});
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDown3.ForeColor = System.Drawing.SystemColors.MenuText;
            this.numericUpDown3.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown3.Location = new System.Drawing.Point(64, 584);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown3.TabIndex = 30;
            this.numericUpDown3.Value = new decimal(new int[] {
            1300,
            0,
            0,
            0});
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDown2.ForeColor = System.Drawing.SystemColors.MenuText;
            this.numericUpDown2.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown2.Location = new System.Drawing.Point(64, 558);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown2.TabIndex = 29;
            this.numericUpDown2.Value = new decimal(new int[] {
            1250,
            0,
            0,
            0});
            // 
            // numericUpDown7
            // 
            this.numericUpDown7.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDown7.ForeColor = System.Drawing.SystemColors.MenuText;
            this.numericUpDown7.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown7.Location = new System.Drawing.Point(280, 557);
            this.numericUpDown7.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown7.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown7.Name = "numericUpDown7";
            this.numericUpDown7.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown7.TabIndex = 40;
            this.numericUpDown7.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDown6.ForeColor = System.Drawing.SystemColors.MenuText;
            this.numericUpDown6.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown6.Location = new System.Drawing.Point(280, 583);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown6.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown6.TabIndex = 39;
            this.numericUpDown6.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDown1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(281, 610);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 37;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(586, 596);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(52, 20);
            this.label18.TabIndex = 43;
            this.label18.Text = "OCR :";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(642, 599);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(312, 20);
            this.textBox3.TabIndex = 42;
            // 
            // but_analysis
            // 
            this.but_analysis.Location = new System.Drawing.Point(730, 622);
            this.but_analysis.Name = "but_analysis";
            this.but_analysis.Size = new System.Drawing.Size(125, 28);
            this.but_analysis.TabIndex = 49;
            this.but_analysis.Text = "Analyse";
            this.but_analysis.UseVisualStyleBackColor = true;
            this.but_analysis.Click += new System.EventHandler(this.But_analysis_Click);
            // 
            // but_set
            // 
            this.but_set.Location = new System.Drawing.Point(433, 594);
            this.but_set.Name = "but_set";
            this.but_set.Size = new System.Drawing.Size(125, 28);
            this.but_set.TabIndex = 50;
            this.but_set.Text = "Set Parameters";
            this.but_set.UseVisualStyleBackColor = true;
            this.but_set.Click += new System.EventHandler(this.But_set_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1304, 574);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 44);
            this.button1.TabIndex = 51;
            this.button1.Text = "testJB";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // cb_continue
            // 
            this.cb_continue.AutoSize = true;
            this.cb_continue.Location = new System.Drawing.Point(144, 254);
            this.cb_continue.Name = "cb_continue";
            this.cb_continue.Size = new System.Drawing.Size(94, 17);
            this.cb_continue.TabIndex = 52;
            this.cb_continue.Text = "Prise Continue";
            this.cb_continue.UseVisualStyleBackColor = true;
            this.cb_continue.CheckedChanged += new System.EventHandler(this.CheckBox2_CheckedChanged);
            // 
            // but_reset
            // 
            this.but_reset.Enabled = false;
            this.but_reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.but_reset.ForeColor = System.Drawing.Color.Red;
            this.but_reset.Location = new System.Drawing.Point(480, 229);
            this.but_reset.Name = "but_reset";
            this.but_reset.Size = new System.Drawing.Size(226, 32);
            this.but_reset.TabIndex = 53;
            this.but_reset.Text = "Relancer le Test";
            this.but_reset.UseVisualStyleBackColor = true;
            this.but_reset.Click += new System.EventHandler(this.But_reset_Click);
            // 
            // numericUpDown8
            // 
            this.numericUpDown8.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDown8.ForeColor = System.Drawing.SystemColors.MenuText;
            this.numericUpDown8.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown8.Location = new System.Drawing.Point(281, 635);
            this.numericUpDown8.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown8.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown8.Name = "numericUpDown8";
            this.numericUpDown8.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown8.TabIndex = 54;
            // 
            // blancNoir
            // 
            this.blancNoir.AutoSize = true;
            this.blancNoir.Location = new System.Drawing.Point(433, 633);
            this.blancNoir.Name = "blancNoir";
            this.blancNoir.Size = new System.Drawing.Size(77, 17);
            this.blancNoir.TabIndex = 56;
            this.blancNoir.Text = "Blanc/Noir";
            this.blancNoir.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(223, 636);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 57;
            this.label3.Text = "Seuil :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(216, 613);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 58;
            this.label12.Text = "Contraste :";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(199, 584);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(75, 13);
            this.label17.TabIndex = 59;
            this.label17.Text = "Largeur OCR :";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(190, 559);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(84, 13);
            this.label19.TabIndex = 60;
            this.label19.Text = "Longueur OCR :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1443, 694);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.blancNoir);
            this.Controls.Add(this.numericUpDown8);
            this.Controls.Add(this.but_reset);
            this.Controls.Add(this.cb_continue);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.but_set);
            this.Controls.Add(this.but_analysis);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.numericUpDown7);
            this.Controls.Add(this.numericUpDown6);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.numericUpDown5);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.CapButton);
            this.Controls.Add(this.DispPictureBox1);
            this.Controls.Add(this.but_close);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.but_test);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nb_plateau)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DispPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button but_grid;
        private System.Windows.Forms.TextBox tb_gcode_in;
        private System.Windows.Forms.Button but_test;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_zpos;
        private System.Windows.Forms.TextBox tb_ypos;
        private System.Windows.Forms.TextBox tb_xpos;
        private System.Windows.Forms.TextBox tb_airpump;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tb_cmd;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox tb_machine;
        private System.IO.Ports.SerialPort serialPort1;
        private System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cb_camera_port;
        private System.Windows.Forms.ComboBox cb_cnc_port;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button but_port;
        private System.Windows.Forms.TextBox tb_gcode_out;
        private System.Windows.Forms.Button but_close;
        private Processus inProcess = Processus.Retrieving;
        private States currentState = States.Idle;
        private States nextState = States.Idle;
        //private List<String> gcode_cmd;
        //private bool isGood;
        private System.Windows.Forms.PictureBox DispPictureBox1;
        private System.Windows.Forms.Button CapButton;
        private System.Windows.Forms.Button but_arduino;
        private System.Windows.Forms.Button but_cnc;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown7;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button but_analysis;
        private System.Windows.Forms.Button but_set;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cb_continue;
        private System.Windows.Forms.Button but_reset;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cb_arduino_port;
        private System.IO.Ports.SerialPort serialPort3;
        private System.Windows.Forms.NumericUpDown numericUpDown8;
        private System.Windows.Forms.CheckBox blancNoir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_x_bon;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_y_bad;
        private System.Windows.Forms.TextBox tb_x_bad;
        private System.Windows.Forms.TextBox tb_y_bon;
        private System.Windows.Forms.NumericUpDown nb_plateau;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tb_etat;
    }
}

