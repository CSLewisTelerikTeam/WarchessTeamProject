﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BoardGame.UnitClasses
{
    class AllianceSquire : RaceAlliance
    {
        //Attack & Health start values
        private const double InitialAttackLevel = 2;
        private const double InitialHealthLevel = 4;

       
        //Unit constructor
        public AllianceSquire(double col, double row)
        {
            this.Type = UnitTypes.Squire;
            this.AttackLevel = InitialAttackLevel;
            this.HealthLevel = InitialHealthLevel;
            this.CounterAttackLevel = InitialAttackLevel / 2;
            this.CurrentPosition = new Point(col, row);

            this.SmallImage = new Image();
            this.BigImage = new Image();

            var path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\squire_small.png");
            this.SmallImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            path = System.IO.Path.GetFullPath(@"..\..\Resources\Alliance\Frames\squire_big.png");
            this.BigImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        public override bool IsMoveable(Point destination)
        {
            //Check if the destination cell is not busy of unit            

            //for (int i = 0; i < InitializedTeams.TeamsCount; i++)
            //{
            //    if ((destination.X == InitializedTeams.AllianceTeam[i].CurrentPosition.X && destination.Y == InitializedTeams.AllianceTeam[i].CurrentPosition.Y) ||
            //        (destination.X == InitializedTeams.HordeTeam[i].CurrentPosition.X && destination.Y == InitializedTeams.HordeTeam[i].CurrentPosition.Y))
            //    {
            //        return false;
            //    }
            //}

            double deltaCol = destination.X - this.CurrentPosition.X;
            double deltaRow = destination.Y - this.CurrentPosition.Y;

            //invalid move
            if (Math.Abs(deltaCol) > 1 || Math.Abs(deltaRow) > 1)
            {
                return false;
            }
            return true;
        }


    }
}
