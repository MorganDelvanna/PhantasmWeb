using System.Collections.Generic;

namespace Phantasm_Parser
{
    class Game
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public int Slot { get; set; }
        public string GameType { get; set; }
        public int StoryTellerId { get; set; }
        public string StoryTellerName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    class BoardGame
    {
        public string Name { get; set; }
    }

    class StoryTeller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    class Day
    {
        public int Id { get; set; }
        public string Weekday { get; set; }
        public List<Slot> Slots { get; set; }
    }

    class Slot
    {
        public int Id { get; set; }
        public string name { get; set; }
        public List<Game> games { get; set; }

        public Slot()
        {
            games = new List<Game>();
        }
    }

    class Schedule
    {
        public List<Day> days { get; set; }
        public List<BoardGame> boardgame { get; set; }
        public List<StoryTeller> storytellers { get; set; }
    }
}
