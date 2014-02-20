using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BoardGame.UnitClasses
{
    //Abstract class Unit, parent for both races and all units. 
    //Unit class can't make any instances

    public abstract class Unit : IMoveable
    {
        //Private Fields 
        private double healthLevel;
        private double attackLevel;
        private double counterAttackLevel;
        private Image smallImage;
        private Image bigImage;

        public UnitTypes Type { get; set; }

        //Properties over private fields, in case need of data validation
        public double HealthLevel
        {
            get {return this.healthLevel;}
            set {this.healthLevel = value; }
        }

        public double AttackLevel
        {
            get { return this.attackLevel; }
            set { this.attackLevel = value; }
        }

        public Point CurrentPosition { get; set; }
        public bool IsSelected { get; set; }

        public Image SmallImage
        {
            get
            {
                return this.smallImage;
            }
            set
            {
                this.smallImage = value;
            }
        }
        public Image BigImage
        {
            get
            {
                return this.bigImage;
            }
            set
            {
                this.bigImage = value;
            }
        }

        public abstract bool IsMoveable(Point destination);
    }
}
