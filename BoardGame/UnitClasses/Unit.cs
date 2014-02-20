using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using OOPGameWoWChess;

namespace BoardGame.UnitClasses
{
    //Abstract class Unit, parent for both races and all units. 
    //Unit class can't make any instances

    public abstract class Unit : IMoveable, IAttacking
    {
        //Private Fields 
        private double healthLevel;
        private double attackLevel;        
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

        public double CounterAttackLevel { get; set; }

        public int Level { get; set; }

        public int Wins { get; set; }

        public int Loses { get; set; }

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

        public void Attack(Unit targetUnit)
        {  
            //Check if the aggresssor could reach the target
            if (this.IsCorrectMove(targetUnit.CurrentPosition) && this.IsClearWay(targetUnit.CurrentPosition))
            {
                targetUnit.HealthLevel -= this.AttackLevel;
                this.HealthLevel -= targetUnit.CounterAttackLevel;

                if (this.HealthLevel <=0 && targetUnit.HealthLevel <=0)
                {
                    targetUnit.SmallImage.Visibility = Visibility.Hidden;
                    targetUnit.CurrentPosition = new Point(-1, -1);

                    this.SmallImage.Visibility = Visibility.Hidden;
                    this.CurrentPosition = new Point(-1, -1);
                    return;                        
                }

                if (targetUnit.HealthLevel <= 0)
                {
                    this.Level++;
                    targetUnit.SmallImage.Visibility = Visibility.Hidden;
                    targetUnit.CurrentPosition = new Point(-1, -1);
                }

                if (this.HealthLevel <= 0)
                {
                    targetUnit.Level++;
                    this.SmallImage.Visibility = Visibility.Hidden;
                    this.CurrentPosition = new Point(-1, -1);
                }
                
            }

        }
        
        
    }
}
