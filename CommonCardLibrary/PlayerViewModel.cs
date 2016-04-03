using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonCardLibrary.Entities;
using Newtonsoft.Json;

namespace CommonCardLibrary
{
    public class PlayerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Position { get; set; }
        public List<Card> Cards { get; set; }
        public decimal TotalCash { get; set; }
        public decimal CashInHand { get; set; }
        public decimal CurrentBet { get; set; }
        public bool Waiting { get; set; }
        public Hand Hand { get; set; }
        public int HandStrength { get; set; }
        public bool Active { get; set; }
        public bool Acting { get; set; }
        public bool Won { get; set; }
        public PlayerViewModel(string name, byte position) 
        {
            Name = name;
            Position = position;
            InitializePlayer();
        }

        public PlayerViewModel()
        {
           InitializePlayer();
        }

        public PlayerViewModel(PlayerHand player, string name)
        {
            Id = player.PlayerId;
            Active = player.Active;
            Acting = player.Acting;
            CurrentBet = player.CurrentBet;
            Waiting = player.Waiting;
            Hand = Hand.HighCard;
            Cards = !string.IsNullOrEmpty(player.Cards)
                ? JsonConvert.DeserializeObject<List<Card>>(player.Cards)
                : new List<Card>();
            TotalCash = player.TotalCash;
            CashInHand = player.CashIn;
            Name = name;
            Won = player.Won;
        }

        private void InitializePlayer()
        {
            Id = Guid.NewGuid();
            Active = true;
            Hand = Hand.HighCard;
            Cards = new List<Card>();
        }
    }

    
}
