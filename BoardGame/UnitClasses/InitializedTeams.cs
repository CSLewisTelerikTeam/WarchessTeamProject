using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.UnitClasses
{
    class InitializedTeams
    {
        //Alliance playing units. These instances will be used in the game.
        public static List<RaceAlliance> AllianceTeam;

        //Horde playing units. These instances will be used in the game.
        public static List<RaceHorde> HordeTeam;

        static InitializedTeams()
        {
            InitializedTeams.AllianceTeam = new List<RaceAlliance>
            {
                new AllianceSquire(0, 6),
                new AllianceSquire(1, 6),
                new AllianceSquire(2, 6),
                new AllianceSquire(3, 6),
                new AllianceSquire(4, 6),
                new AllianceSquire(5, 6),
                new AllianceSquire(6, 6),
                new AllianceSquire(7, 6),
                new AllianceMage(1, 7),
                new AllianceMage(6, 7),
                new AllianceCaptain(2, 7),
                new AllianceCaptain(5, 7),
                new AllianceWarGolem(0, 7),
                new AllianceWarGolem(7, 7),
                new AlliancePriest(3, 7),
                new AllianceKing(4, 7),
            };

            InitializedTeams.HordeTeam = new List<RaceHorde>
            {
                new HordeGrunt(0, 1),
                new HordeGrunt(1, 1),
                new HordeGrunt(2, 1),
                new HordeGrunt(3, 1),
                new HordeGrunt(4, 1),
                new HordeGrunt(5, 1),
                new HordeGrunt(6, 1),
                new HordeGrunt(7, 1),
                new HordeWarlock(1, 0),
                new HordeWarlock(6, 0),
                new HordeCommander(2, 0),
                new HordeCommander(5, 0),
                new HordeDemolisher(0, 0),
                new HordeDemolisher(7, 0),
                new HordeShaman(3, 0),
                new HordeWarchief(4, 0),
            };
        }
    }
}