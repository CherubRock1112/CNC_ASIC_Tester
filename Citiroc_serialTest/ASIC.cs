using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citiroc_serialTest
{

    //La classe ASIC représente une case du plateau
    public class ASIC
    {
        #region Attributs

        private float _x, _y, _z;
        private bool _isGood, _isOccupied;
        private String _serial;

        #endregion

        #region Accesseurs
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }
        public String Serial
        {
            get { return _serial; }
            set { _serial = value; }
        }

        public bool isGood
        {
            get { return _isGood; }
            set { _isGood = value; }
        }
        public bool isOccupied
        {
            get { return _isOccupied; }
            set { _isOccupied = value; }
        }
        #endregion

        #region Constructeurs
        public ASIC()
        {
            this._x = 0;
            this._y = 0;
            this._serial = "";
        }

        public ASIC(float x, float y)
        {
            this._x = x;
            this._y = y;
            this._serial = "";
        }

        public ASIC(float x, float y, float z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public ASIC(float x, float y, float z, bool isOccupied)
        {
            this._x = x;
            this._y = y;
            this._z = z;
            this._isOccupied = isOccupied;
        }
        #endregion region

        #region Méthodes
        public String getPositionningCmd()
        {
            return "G00X" + _x.ToString() + "Y" + _y.ToString();
        }


        public String getLoweringCmd()
        {
            float temp = _z + 10;
            return "G00Z" + temp.ToString();
        }

        public String getTouchCmd()
        {
            return "G00Z" + _z.ToString();
        }

        public String getUpperingCmd()
        {
            float temp = (_z + 40 > 0 ? 0 : _z + 40); //Lève de 4cm, met à 0 si le résultat est supérieur à 0
            return "G00Z" + temp;
        }
        #endregion


    }
}
