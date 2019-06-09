using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public enum Card : int { // C: Clubs, D: Diamonds, H: Hearts, S: Spades
	AceC=0, TwoC, ThreeC, FourC, FiveC, SixC, SevenC, EightC, NineC, TenC, JackC, QueenC, KingC,
	AceD, TwoD, ThreeD, FourD, FiveD, SixD, SevenD, EightD, NineD, TenD, JackD, QueenD, KingD,
	AceH, TwoH, ThreeH, FourH, FiveH, SixH, SevenH, EightH, NineH, TenH, JackH, QueenH, KingH,
	AceS, TwoS, ThreeS, FourS, FiveS, SixS, SevenS, EightS, NineS, TenS, JackS, QueenS, KingS
}

public class WarCardGameManager : MonoBehaviour {

	// Game parameters
	private int cardsInHand = 13;
	private int cardsTotal = 26;

	// Score state
	private int playerScore = 0;
	private int opponentScore = 0;

	// All available Cards
	List<Card> cards = new List<Card>{
		Card.AceC, Card.TwoC, Card.ThreeC, Card.FourC, Card.FiveC, Card.SixC, Card.SevenC, 
		Card.EightC, Card.NineC, Card.TenC, Card.JackC, Card.QueenC, Card.KingC,
		Card.AceD, Card.TwoD, Card.ThreeD, Card.FourD, Card.FiveD, Card.SixD, Card.SevenD, Card.EightD, Card.NineD, Card.TenD, Card.JackD, Card.QueenD, Card.KingD,
		Card.AceH, Card.TwoH, Card.ThreeH, Card.FourH, Card.FiveH, Card.SixH, Card.SevenH, Card.EightH, Card.NineH, Card.TenH, Card.JackH, Card.QueenH, Card.KingH,
		Card.AceS, Card.TwoS, Card.ThreeS, Card.FourS, Card.FiveS, Card.SixS, Card.SevenS, Card.EightS, Card.NineS, Card.TenS, Card.JackS, Card.QueenS, Card.KingS
	};

	// Players state
	List<Card> playerDeck = null;
	List<Card> opponentDeck = null;
	List<Card> playerHand = null;
	List<Card> opponentHand = null;

	// UI elements
	public List<GameObject> playerHandObjects = new List<GameObject>();
	public List<GameObject> opponentHandObjects = new List<GameObject>();
	public GameObject playerPlayedCard = null;
	public GameObject opponentPlayedCard = null;
	public GameObject playerDeckObject = null;
	public GameObject opponentDeckObject = null;
	public GameObject opponentFinger = null;

	// FSM
	public PlayMakerFSM playerFsm = null;
	public PlayMakerFSM opponentFsm = null;
	public PlayMakerFSM backgroundFsm = null;

	void Start () { 
		assignCards();
	}

	void Update () { }

	#region Setup

	public void startGame() {
		// Score reset
		this.playerScore = 0;
		this.opponentScore = 0;

		// Set decks to full
		this.opponentDeckObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("WarGameAssets/WarGame_DeckFull");
		this.playerDeckObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("WarGameAssets/WarGame_DeckFull");

		// Start Moving ooponent finger
		animateOpponentFinger();
	}

	// We shuffle the deck, then we assign first N cards to each player
	void assignCards() {
		this.playerDeck = new List<Card>(this.cards);
		this.opponentDeck = new List<Card>(this.cards);

		// Shuffle decks
		for (int t = 0; t < this.playerDeck.Count; t++) {
			// Player
            Card playerTemp = this.playerDeck[t];
            int r = UnityEngine.Random.Range(t, this.playerDeck.Count);
            this.playerDeck[t] = this.playerDeck[r];
            this.playerDeck[r] = playerTemp;
			// Opponent
            Card opponentTemp = this.opponentDeck[t];
            r = UnityEngine.Random.Range(t, this.opponentDeck.Count);
            this.opponentDeck[t] = this.opponentDeck[r];
            this.opponentDeck[r] = opponentTemp;
        }

		// Assing first N cards to each player hand
		this.playerHand = this.playerDeck.GetRange(0, this.cardsInHand);
		this.opponentHand = this.opponentDeck.GetRange(0, this.cardsInHand);

		// Order cards from Lowest to Highest
		this.playerHand = sortCardsList(this.playerHand);
		cardsListToString(this.playerHand);
		this.opponentHand = sortCardsList(this.opponentHand);
		cardsListToString(this.opponentHand);

		// Remove the picked cards from both decks
		int cardsLeft = this.cardsTotal - this.cardsInHand;
		this.playerDeck = this.playerDeck.GetRange(this.cardsInHand, cardsLeft);
		this.opponentDeck = this.opponentDeck.GetRange(this.cardsInHand, cardsLeft);

		// Set UI for player
		for (int i = 0; i < this.cardsInHand; i++) {
			Card card = this.playerHand[i];
			GameObject cardObject = this.playerHandObjects[i];
			cardObject.GetComponent<SpriteRenderer>().sprite = spriteForCard(card);
		}
	}

	public void restartGame() {
		assignCards();
		startGame();

		for (int i = 0; i <  this.playerHandObjects.Count; i++) {
			GameObject cardObject = this.playerHandObjects[i];
			cardObject.SetActive(true);
			cardObject.GetComponent<PlayMakerFSM>().SendEvent("AssignNewCard");

			GameObject cardObjectOpponet = this.opponentHandObjects[i];
			cardObjectOpponet.SetActive(true);
			cardObjectOpponet.GetComponent<PlayMakerFSM>().SendEvent("FadeCardIn");
		}

		animateOpponentFinger();
	}

	#endregion

	#region Gameplay

	public void playCard(int playerCardIndex) {
		Debug.Log("Player playing Card at Index " + playerCardIndex);

		// Get the card from the player hand, then remove it
		Card playerCard = this.playerHand[playerCardIndex];
		this.playerHand.RemoveAt(playerCardIndex);

		// Get opponent card
		Card opponentCard = getOpponentCard();

		// Show the two cards on the table
		showPlayerCard(playerCard);
		showOpponentCard(opponentCard);

		assignPoint(playerCard, opponentCard);
	}

	public void nextRound() {
		// Either game is over or new cards need to be assigned
		if (isGameOver()) {
			gameOver();
		} else {
			drawCards(this.playerDeck, ref this.playerHand, this.playerFsm);
			drawCards(this.opponentDeck, ref this.opponentHand, this.opponentFsm);

			refreshPlayerHand();
			refreshOpponentHand();
			animateOpponentFinger();
		}
	}

	Card getOpponentCard() {
		iTween.Stop(this.opponentFinger);

		int closestCardIndex = 0;
		float closestCardDistance = 1000f; // Just a very big number
		// Get the x coordinate of the finger
		float cardSize = this.opponentHandObjects.First().GetComponent<RectTransform>().rect.width;
		float handX = this.opponentFinger.transform.position.x + this.opponentFinger.GetComponent<RectTransform>().rect.width;
		// Find the closes card - using the left/top corner as Center
		for (int i = 0; i <= lastActiveOpponentCardIndex(); i++) {
			float cardX = this.opponentHandObjects[i].transform.position.x - cardSize/2f;
			float distance = handX - cardX;
			if (distance > 0 && distance < closestCardDistance) {
				closestCardIndex = i;
				closestCardDistance = distance;
			}
		}
		Debug.Log("Opponent picking card at index: " + closestCardIndex);
		this.opponentHandObjects[closestCardIndex].GetComponent<PlayMakerFSM>().SendEvent("FadeCardOut");
		Card opponentCard = this.opponentHand[closestCardIndex];
		this.opponentHand.RemoveAt(closestCardIndex);
		return opponentCard;	
	}

	// Change the Player Card on the table, then Fade it in
	void showPlayerCard(Card playerCard) {
		this.playerPlayedCard.GetComponent<SpriteRenderer>().sprite = spriteForCard(playerCard);
		this.playerPlayedCard.GetComponent<PlayMakerFSM>().SendEvent("Fade Card In");
	}

	void showOpponentCard(Card opponentCard) {
		this.opponentPlayedCard.GetComponent<SpriteRenderer>().sprite = spriteForCard(opponentCard);
		this.opponentPlayedCard.GetComponent<PlayMakerFSM>().SendEvent("Fade Card In");
	}

	void assignPoint(Card playerCard, Card opponentCard) {
		int playerCardValue = (int)playerCard % 13;
		int opponentCardValue = (int)opponentCard % 13;

		// Show winner label and increase score
		if (playerCardValue > opponentCardValue) {
			this.playerScore++;
			this.playerFsm.SendEvent("winCardRound");
		} else if (playerCardValue < opponentCardValue) {
			this.opponentScore++;
			this.opponentFsm.SendEvent("winCardRound");
		} else { // Duce
			this.backgroundFsm.SendEvent("TieCardRound");
		}
	}

	// Get a new card from deck into both players hand
	void drawCards(List<Card> deck, ref List<Card> hand, PlayMakerFSM fsm) {
		// We get the first card from the deck - If any is left
		if (deck.Count <= 0) { return; }
		
		// Get a card from the deck and remove it
		Card newCard = deck[0];
		deck.RemoveAt(0); 

		// Add the card to the hand and sort the cards in order
		hand.Add(newCard);
		hand = sortCardsList(hand);
		cardsListToString(hand);
	}

	void refreshPlayerHand() {
		for (int i = 0; i < this.cardsInHand; i++) {
			if (i < this.playerHand.Count) {
				Card card = this.playerHand[i];
				GameObject cardObject = this.playerHandObjects[i];
				cardObject.GetComponent<SpriteRenderer>().sprite = spriteForCard(card);
				cardObject.GetComponent<PlayMakerFSM>().SendEvent("AssignNewCard");
			} else {
				this.playerHandObjects[i].SetActive(false);
			}
		}
		this.playerDeckObject.GetComponent<SpriteRenderer>().sprite = spriteForDeck(this.playerDeck);
	}

	void refreshOpponentHand() {
		for (int i = 0; i < this.cardsInHand; i++) {
			if (i < this.opponentHand.Count) {
				Card card = this.opponentHand[i];
				GameObject cardObject = this.opponentHandObjects[i];
				cardObject.GetComponent<PlayMakerFSM>().SendEvent("FadeCardIn");
			} else {
				this.opponentHandObjects[i].SetActive(false);
			}
		}
		this.opponentDeckObject.GetComponent<SpriteRenderer>().sprite = spriteForDeck(this.opponentDeck);
	}

	void animateOpponentFinger() {
		float cardSize = this.opponentHandObjects.First().GetComponent<RectTransform>().rect.width;
		float xStart = this.opponentHandObjects.First().transform.position.x - cardSize/2f;
		float xEnd = this.opponentHandObjects[lastActiveOpponentCardIndex()].transform.position.x + cardSize/2f;
		float currentHandX = this.opponentFinger.transform.position.x;
		float finalHandX = UnityEngine.Random.Range(xStart + 0.5f, xEnd - 0.5f);
		float time = Math.Abs(currentHandX - finalHandX) / 2.5f; 
		iTween.MoveTo(this.opponentFinger, iTween.Hash("x", finalHandX, "time", time,"delay", .5, "easeType", "linear", "oncomplete", "OnMoveComplete", "oncompletetarget", gameObject));
	}

	public void OnMoveComplete() {
		animateOpponentFinger();
	}

	void gameOver() {
		// Trigger one of the two ends state based on who won
		if (this.playerScore >= this.opponentScore) {
			backgroundFsm.SendEvent("PlayerWonCardGame");
		} else {
			backgroundFsm.SendEvent("PlayerLostCardGame");
		}
	}

	bool isGameOver() {
		return (this.playerHand.Count == 0 && this.opponentHand.Count == 0);
	}

	#endregion

	#region Helpers

	List<Card> sortCardsList(List<Card> list) {
		List<Card> sortedList = list.OrderBy(o=>((int)o)%13).ToList();
		return sortedList;
	}

	Sprite spriteForCard(Card card) {
		Sprite sprite = Resources.Load<Sprite>("WarGameAssets/WarGame_" + (int)card);
		return sprite;
	}

	Sprite spriteForDeck(List<Card> deck) {
		if (deck.Count >= 9) {
			return Resources.Load<Sprite>("WarGameAssets/WarGame_DeckFull");
		} else if (deck.Count >= 5) {
			return Resources.Load<Sprite>("WarGameAssets/WarGame_DeckMedium");
		} else if (deck.Count >= 1) {
			return Resources.Load<Sprite>("WarGameAssets/WarGame_DeckLow");
		} else {
			return null;
		}
	}

	int lastActiveOpponentCardIndex() {
		for (int i = this.opponentHandObjects.Count - 1; i >= 0; i--) {
            if (this.opponentHandObjects[i].activeSelf) { return i; }
		}
		return 0;
	}

	void cardsListToString(List<Card> list) {
		Debug.Log(string.Join(",", list.Select(s => ((int)s%13).ToString()).ToList().ToArray()));
	}

	#endregion

}
