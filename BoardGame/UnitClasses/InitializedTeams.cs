namespace BoardGame.UnitClasses
{
    using System.Collections.Generic;
    using System.Windows;
    
    class InitializedTeams
    {
        public const int TeamsCount = 16;

        //Alliance playing units. These instances will be used in the game.
        public static List<RaceAlliance> AllianceTeam;

        //Horde playing units. These instances will be used in the game.
        public static List<RaceHorde> HordeTeam;

        static InitializedTeams()
        {
            InitializedTeams.AllianceTeam = new List<RaceAlliance>
            {
                new AllianceSquire(new Point(0, 6)),
                new AllianceSquire(new Point(1, 6)),
                new AllianceSquire(new Point(2, 6)),
                new AllianceSquire(new Point(3, 6)),
                new AllianceSquire(new Point(4, 6)),
                new AllianceSquire(new Point(5, 6)),
                new AllianceSquire(new Point(6, 6)),
                new AllianceSquire(new Point(7, 6)),
                new AllianceMage(new Point(1, 7)),
                new AllianceMage(new Point(6, 7)),
                new AllianceCaptain(new Point(2, 7)),
                new AllianceCaptain(new Point(5, 7)),
                new AllianceWarGolem(new Point(0, 7)),
                new AllianceWarGolem(new Point(7, 7)),
                new AlliancePriest(new Point(3, 7)),
                new AllianceKing(new Point(4, 7)),
            };

            InitializedTeams.HordeTeam = new List<RaceHorde>
            {
                new HordeGrunt(new Point(0, 1)),
                new HordeGrunt(new Point(1, 1)),
                new HordeGrunt(new Point(2, 1)),
                new HordeGrunt(new Point(3, 1)),
                new HordeGrunt(new Point(4, 1)),
                new HordeGrunt(new Point(5, 1)),
                new HordeGrunt(new Point(6, 1)),
                new HordeGrunt(new Point(7, 1)),
                new HordeWarlock(new Point(1, 0)),
                new HordeWarlock(new Point(6, 0)),
                new HordeCommander(new Point(2, 0)),
                new HordeCommander(new Point(5, 0)),
                new HordeDemolisher(new Point(0, 0)),
                new HordeDemolisher(new Point(7, 0)),
                new HordeShaman(new Point(3, 0)),
                new HordeWarchief(new Point(4, 0)),
            };
        }
    }
}