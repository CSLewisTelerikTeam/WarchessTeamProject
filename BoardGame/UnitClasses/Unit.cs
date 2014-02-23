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
        private Image smallImage;
        private Image bigImage;
        protected MediaPlayer playSound = new MediaPlayer();

        //Properties over private fields, in case need of data validation
        public UnitTypes Type { get; set; }
        public RaceTypes Race { get; set; }
        public int HealthLevel { get; set;}
        public int InitialHealthLevel { get; set;}
        public int MaxHealthLevel { get; set;}
        public int AttackLevel { get; set; }
        public int InitialAttackLevel { get; set; }
        public int MaxAttackLevel { get; set; }
        public int CounterAttackLevel { get; set; }
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
        public Unit(UnitTypes type, RaceTypes race,
            int initialHealthLevel, int initialAttackLevel,
            int level, bool isAlive, Point currentPosition, bool isSelected)
        {
            this.Type = type;
            this.Race = race;

            //Current health level, initial health level and max health level are the same int the initialization
            this.HealthLevel = initialHealthLevel;
            this.InitialHealthLevel = initialHealthLevel;
            this.MaxHealthLevel = initialHealthLevel;
            
            //Current attack level, initial attack level and max attack level are the same int the initialization
            this.AttackLevel = initialAttackLevel;
            this.InitialAttackLevel = initialAttackLevel;
            this.MaxAttackLevel = initialAttackLevel;
            this.CounterAttackLevel = initialAttackLevel / 2;

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
                    targetUnit.IsAlive = false;

                    //Level up the selected unit and at the same time up its attack and health
                    this.Level++;
                    this.MaxAttackLevel++;
                    this.AttackLevel++;
                    this.MaxHealthLevel++;
                    this.HealthLevel++;

                    targetUnit.SmallImage.Source = new BitmapImage();
                    (targetUnit.SmallImage.Parent as Border).Background = null;
                    
                    this.CurrentPosition = targetUnit.CurrentPosition;
                    targetUnit.CurrentPosition = new Point(-1, -1);

                    Grid.SetRow(this.SmallImage.Parent as Border, (int)this.CurrentPosition.Y);
                    Grid.SetColumn(this.SmallImage.Parent as Border, (int)this.CurrentPosition.X);
                }

                if (this.HealthLevel <= 0)
                {
                    this.PlayDieSound();
                    
                    //Level up the selected unit and at the same time up its attack and health
                    targetUnit.Level++;
                    targetUnit.MaxAttackLevel++;
                    targetUnit.AttackLevel++;
                    targetUnit.MaxHealthLevel++;
                    targetUnit.HealthLevel++;

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
