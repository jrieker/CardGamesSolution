using CardGamesSolution.Server.Blackjack;
using CardGamesSolution.Server.Shared;
using System.Collections.Generic;

namespace CardGamesSolution.Tests
{
    [TestClass]
    public class BlackJackEngineTests
    {
        private BlackJackEngine engine;

        [TestInitialize]
        public void Setup()
        {
            engine = new BlackJackEngine();
        }


        [TestMethod]
        public void DealHands_Correctly()
        {
            // Arrange
            Deck deck = new Deck();

            var players = new List<Player>
            {
                new Player(1, "Alice", 100),
                new Player(2, "Bob",   100)
            };

            Hand dealersHand = new Hand();

            // Act
            engine.DealHands(players, dealersHand, deck);

            // Assert
            //Each player has exactly 2 cards
            foreach (var player in players)
            {
                Assert.AreEqual(2, player.PlayerHand.getCards().Count);
            }

            //Dealers has two cards
            Assert.AreEqual(2, dealersHand.getCards().Count);
        }

        [TestMethod]
        public void HitWorks_Correctly()
        {
            // Arrange
            Hand hand = new Hand();
            Deck deck = new Deck();

            //act
            int returnedValue = engine.Hit(hand, deck);

            //Assert
            //values are eqal
            Assert.AreEqual(hand.valueOfHand(), returnedValue);

            var cards = hand.getCards();
            int computedSum = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                computedSum += cards[i].Number;
            }
            //assert values are equal
            Assert.AreEqual(1, cards.Count);

        }

        [TestMethod]
        public void DealerStopAt17_Correctly()
        {
            // Arrange
            var dealerHand = new Hand();
            dealerHand.AddCard(new Card("Hearts", 10));
            var deck = new Deck();

            int beforeCount = dealerHand.getCards().Count;
            int beforeValue = dealerHand.valueOfHand();

            // Act
            int finalValue = engine.PlayDealer(dealerHand, deck);

            // Assert
            Assert.AreEqual(dealerHand.valueOfHand(), finalValue);
            Assert.IsTrue(finalValue >= 17);

            int afterCount = dealerHand.getCards().Count;
            Assert.IsTrue(afterCount > beforeCount);
        }

        [TestMethod]
        public void DealerStepDraw_NoNonBustedPlayers_DealerDrawsThenStops()
        {
            // Arrange
            //set up dealer
            Hand dealerHand = new Hand();
            dealerHand.AddCard(new Card("Hearts", 10));

            //set up a busted plater
            var bustedPlayer = new Player(1, "busted", 100f);
            //player with more than 21
            bustedPlayer.PlayerHand.AddCard(new Card("Hearts", 22));

            //list of busted playesr
            var players = new List<Player> { bustedPlayer };
            var deck = new Deck();


            int beforeCount = dealerHand.getCards().Count;
            int beforeValue = dealerHand.valueOfHand();

            // Act
            var (card, value, shouldContinue) = engine.DealerStepDraw(dealerHand, deck, players);

            // Assert
            Assert.IsNotNull(card);
            Assert.AreEqual(beforeCount + 1, dealerHand.getCards().Count);
            Assert.AreEqual(dealerHand.valueOfHand(), value);
            Assert.IsTrue(value > beforeValue);
            Assert.IsFalse(shouldContinue);
        }

        [TestMethod]
        public void DealerStepDraw_TieAtOver17_PushAndStop()
        {
            //Arrange
            var dealerHand = new Hand();
            dealerHand.AddCard(new Card("Hearts", 10));
            dealerHand.AddCard(new Card("Clubs", 8));

            var player = new Player(1, "dummy", 100);
            player.PlayerHand.AddCard(new Card("Spades", 9));
            player.PlayerHand.AddCard(new Card("Diamonds", 9));

            var players = new List<Player> { player };
            var deck = new Deck();

            int beforeCount = dealerHand.getCards().Count;
            int beforeValue = dealerHand.valueOfHand();

            //Act
            var (drawnCard, newValue, shouldContinue) = engine.DealerStepDraw(dealerHand, deck, players);

            //Assert
            Assert.IsNull(drawnCard);
            Assert.AreEqual(beforeCount, dealerHand.getCards().Count);
            Assert.AreEqual(beforeValue, newValue);
            Assert.IsFalse(shouldContinue);


        }

        [TestMethod]
        public void ComputePayout_VariousScenarios_ReturnsExpected()
        {
            float bet = 50f;

            // Player busts
            Assert.AreEqual(-bet, engine.ComputePayout(22, 10, bet));

            // Dealer busts
            Assert.AreEqual(+bet, engine.ComputePayout(20, 22, bet));

            // Player wins
            Assert.AreEqual(+bet, engine.ComputePayout(19, 17, bet));

            // Player loses
            Assert.AreEqual(-bet, engine.ComputePayout(16, 18, bet));

            // Push
            Assert.AreEqual(0, engine.ComputePayout(18, 18, bet));
        }
    }
}
