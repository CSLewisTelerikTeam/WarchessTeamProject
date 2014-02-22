using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OOPGameWoWChess;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    //Abstract class Unit, parent for both races and all units. 
    //Unit class can't make any instances
    //Some comments added
    //Some other comment
    public abstract class Unit : IMoveable, IAttacking, ISound
    {
        //Private Fields 
        private double healthLevel;
        private double attackLevel;        
        private Image smallImage;
        private Image bigImage;
        protected MediaPlayer playSound = new MediaPlayer();

        //Properties over private fields, in case need of data validation
        public UnitTypes Type { get; set; }
        public RaceTypes Race { get; set; }
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
        public double CounterAttackLevel { get; set; }
        public int Level { get; set; }
        public bool IsAlive { get; set; }
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

        //Constructors
        public Unit(UnitTypes type, RaceTypes race, double healthLevel,
            double attackLevel, double counterAttackLevel, int level,
            bool isAlive, Point currentPosition, bool isSelected)
        {
            this.Type = type;
            this.Race = race;
            this.HealthLevel = healthLevel;
            this.AttackLevel = attackLevel;
            this.CounterAttackLevel = counterAttackLevel;
            this.Level = level;
            this.IsAlive = isAlive;
            this.CurrentPosition = currentPosition;
            this.IsSelected = isSelected;
        }

        //Methods
        public abstract bool IsCorrectMove(Point destination);
        public abstract bool IsClearWay(Point destination);
        public bool IsSomeoneAtThisPosition(Point destination)
        {
            for (int i = 0; i < InitializedTeams.TeamsCount; i++)
            {
                if ((destination.X == InitializedTeams.AllianceTeam[i].CurrentPosition.X && destination.Y == InitializedTeams.AllianceTeam[i].CurrentPosition.Y) ||
                    (destination.X == InitializedTeams.HordeTeam[i].CurrentPosition.X && destination.Y == InitializedTeams.HordeTeam[i].CurrentPosition.Y))
                {
                    return false;
                }
            }
            return true;
        }
        public void Attack(Unit targetUnit, out bool successAttack)
        {
            successAttack = false;
            //Check if the aggresssor could reach the target
            if (this.IsCorrectMove(targetUnit.CurrentPosition) && this.IsClearWay(targetUnit.CurrentPosition))
            {
                successAttack = true;
                targetUnit.HealthLevel -= this.AttackLevel;
                this.PlayAttackSound();

                if (targetUnit.IsCorrectMove(this.CurrentPosition))
                {                    
                    this.HealthLevel -= targetUnit.CounterAttackLevel;                    
                }                

                if (this.HealthLevel <=0 && targetUnit.HealthLevel <=0)
                {
                    this.PlayDieSound();
                    targetUnit.PlayDieSound();

                    targetUnit.SmallImage.Source = new BitmapImage();
                    (targetUnit.SmallImage.Parent as Border).Background = null;
                    targetUnit.CurrentPosition = new Point(-1, -1);

                    this.SmallImage.Source = new BitmapImage();
                    (targetUnit.SmallImage.Parent as Border).Background = null;
                    this.CurrentPosition = new Point(-1, -1);
                    return;                        
                }

                if (targetUnit.HealthLevel <= 0)
                {
                    targetUnit.PlayDieSound();

                    this.Level++;

                    targetUnit.SmallImage.Source = new BitmapImage();
                    (targetUnit.SmallImage.Parent as Border).Background = null;

                    targetUnit.CurrentPosition = new Point(-1, -1);
                }

                if (this.HealthLevel <= 0)
                {
                    this.PlayDieSound();

                    targetUnit.Level++;

                    this.SmallImage.Source = new BitmapImage();
                    (this.SmallImage.Parent as Border).Background = null;

                    this.CurrentPosition = new Point(-1, -1);
                }
                
            }

        }
        public abstract void PlayAttackSound();
        public abstract void PlaySelectSound();
        public abstract void PlayDieSound();
    }
}
