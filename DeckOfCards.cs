using System;
using System.Collections.Generic;

class Card{
	public int value;
	protected string suit;
	public string name;
	
	public Card(int value, string suit){
		this.value = value;
		this.suit = suit;
		
		if(value == 11){
			this.name = "Jack";
		}else if(value==12){
			this.name="Queen";
		}else if(value==13){
			this.name="King";
		}else if(value==1){
			this.name="Ace";
		}else{
			this.name = value.ToString();
		}
	}
	
	public override string ToString(){
		return this.name+" of "+this.suit;
	}
}

class Deck{
	public List<Card> deck;
	
	public Deck(){
		deck = new List<Card>();
		
		string[] suits = new string[]{"Hearts", "Clubs", "Spades", "Diamond"};
		
		for(int s=0; s<suits.Length; s++){
			for(int v = 1; v<14 ; v++){
				Card cardToAdd = new Card(v,suits[s]);
				deck.Add(cardToAdd);
			}
		}
	}
	
	public void PrintAll(){
		for(int idx = 0; idx<deck.Count; idx++){
			Console.WriteLine(deck[idx]);
		}
	}
	
	public void Shuffle(){
		Random r = new Random();
		for(int idx=0; idx<deck.Count; idx++){
			int rand_idx = r.Next(idx, deck.Count);
			Card temp = deck[rand_idx];
			deck[rand_idx] = deck[idx];
			deck[idx] = temp;
		}
	}
	
	public Card Draw(){
		if(this.deck.Count>0){
			Card c = this.deck[0];
			deck.RemoveAt(0);
			return c;
		}
		else
			return null;
	}
}

class Player{
	public string name;
	public long money;
	
	public List<Card> hand;
	
	public Player(string name, long money){
		this.name = name;
		this.money = money;
		
		hand = new List<Card>();
	}
}

class BlackJack{
	public Player dealer;
	public Player player;
	
	Deck d;
	
	public void GetInfo(){
		string name;
		int money;
		Console.WriteLine("Please write your name: ");
		name = (string)Console.ReadLine();
		Console.WriteLine("Enter the initial ammount of money you want to bet");
		money =  Convert.ToInt32(Console.ReadLine());
		player = new Player(name, money);
	}
	
	public BlackJack(){
		Console.WriteLine("Welcome to Black Jack!");
		dealer = new Player("dealer", 10000);
		GetInfo();
		d = new Deck();
	}
	
	public int StartGame(){
		for(int t=0; t<2;t++){
			dealer.hand.Add(d.Draw());
			player.hand.Add(d.Draw());
		}
		Console.WriteLine("How much of "+player.money+" would you like to bet?");
		int bet = Convert.ToInt32(Console.ReadLine());
		while(bet<1){
			Console.WriteLine("Bet cannot be smaller than $2");
			bet = Convert.ToInt32(Console.ReadLine());
		}
		while(bet>player.money){
			Console.WriteLine("Cannot bet more than you have");
			bet = Convert.ToInt32(Console.ReadLine());
		}
		return bet;
	}
	
	public void ShowHand(List<Card> hand, Player p){
		Console.WriteLine(p.name+"'s hand: ");
		for(int i=0; i<hand.Count;i++){
			Console.WriteLine(hand[i]);
		}
		Console.WriteLine();
		ShowStats();
	}
	
	public int SumHand(List<Card> hand){
		int sum = 0;
		int sumAce = 0;
		for(int i=0; i<hand.Count; i++){
			if(hand[i].value>10){
				sum+=10;
			}else if(hand[i].value==1 && sum+11<21){
				sum+=11;
				sumAce++;
			}else{
				sum+=hand[i].value;
			}
		}
		while(sum>21 && sumAce>0){
			sum-=10;
			sumAce--;
		}
		return sum;
	}
	
	public void ShowStats(){
		Console.WriteLine("The deales total card sum is: "+ SumHand(dealer.hand));
		Console.WriteLine("Your total card sum is: "+ SumHand(player.hand));
	}

	
	public void Game(){
		int playerSum = 0;
		int dealerSum = 0;
		bool play = true;
		int bet = StartGame();
		string answer = "";
		d.Shuffle();
		
		
		while(play && player.money>1 && d.deck.Count>4){
			ShowHand(dealer.hand, dealer);
			ShowHand(player.hand, player);
			
			playerSum = SumHand(player.hand);
			dealerSum = SumHand(dealer.hand);
			
			if(playerSum==21){
				Console.WriteLine("BLACK JACK!");
				player.money +=bet*2;
				Console.WriteLine("Would you like to start over? yes/no");
				answer = (string)Console.ReadLine();
				while(answer!="yes" && answer!="no"){
					Console.WriteLine("Please enter yes/no only");
					answer = (string)Console.ReadLine();
				}
				if(answer=="no"){
					Console.WriteLine("There are "+d.deck.Count+" cards left");
					play = false;
				}else{
					dealer.hand.Clear();
					player.hand.Clear();
					bet = StartGame();
				}
			}
			else if(dealerSum==21){
				player.money-=bet;
				Console.WriteLine("You lost, the dealer has BLACK JACK!");
				Console.WriteLine("Would you like to start over? yes/no");
				answer = (string)Console.ReadLine();
				while(answer!="yes" && answer!="no"){
					Console.WriteLine("Please enter yes/no only");
					answer = (string)Console.ReadLine();
				}
				if(answer=="no"){
					Console.WriteLine("There are "+d.deck.Count+" cards left");
					play = false;
				}else{
					dealer.hand.Clear();
					player.hand.Clear();
					bet = StartGame();
				}
			}
			else if(playerSum>21){
				player.money-=bet;
				Console.WriteLine("You lost!");
				Console.WriteLine("Would you like to start over? yes/no");
				answer = (string)Console.ReadLine();
				while(answer!="yes" && answer!="no"){
					Console.WriteLine("Please enter yes/no only");
					answer = (string)Console.ReadLine();
				}
				if(answer=="no"){
					Console.WriteLine("There are "+d.deck.Count+" cards left");
					play = false;
				}else{
					dealer.hand.Clear();
					player.hand.Clear();
					bet = StartGame();
				}
			}else if(dealerSum>21){
				Console.WriteLine("You Won!");
				player.money +=bet*2;
				Console.WriteLine("Would you like to start over? yes/no");
				answer = (string)Console.ReadLine();
				while(answer!="yes" && answer!="no"){
					Console.WriteLine("You entered "+answer);
					Console.WriteLine("Please enter yes/no only");
					answer = (string)Console.ReadLine();
				}
				if(answer=="no"){
					Console.WriteLine("There are "+d.deck.Count+" cards left");
					play = false;
				}else{
					dealer.hand.Clear();
					player.hand.Clear();
					bet = StartGame();
				}
			}
			else{
				Console.WriteLine("Would you like to hit? yes/no");
				answer = (string)Console.ReadLine();
				while(answer!="yes" && answer!="no"){
					Console.WriteLine("Please enter yes/no only");
					answer = (string)Console.ReadLine();
				}
				if(answer=="yes"){
					player.hand.Add(d.Draw());
					ShowHand(player.hand, player);
				}else{
					while(SumHand(dealer.hand)<21 || SumHand(dealer.hand)<SumHand(player.hand)){
						dealer.hand.Add(d.Draw());
						dealerSum = SumHand(dealer.hand);
						ShowHand(dealer.hand, dealer);
						Console.WriteLine("Dealer's total is: "+dealerSum);
					}
					if(SumHand(dealer.hand)>SumHand(player.hand) && SumHand(dealer.hand)<22){
						player.money-=bet;
						Console.WriteLine("Your sum is: "+playerSum);
						Console.WriteLine("Dealer's total is: "+dealerSum);
						Console.WriteLine("You lost!");
					}else{
						player.money +=bet*2;
						Console.WriteLine("You Won!");
					}
					Console.WriteLine("Would you like to start over? yes/no");
					answer = (string)Console.ReadLine();
					while(answer!="yes" && answer!="no"){
						Console.WriteLine("Please enter yes/no only");
						answer = (string)Console.ReadLine();
					}
					if(answer=="no"){
						Console.WriteLine("There are "+d.deck.Count+" cards left");
						play = false;
					}else{
						dealer.hand.Clear();
						player.hand.Clear();
						bet = StartGame();
					}
				}
			}
		}
		if(player.money<1){
			Console.WriteLine("You run out of money!");
		}
		else if(d.deck.Count<=4){
			Console.WriteLine("No more cards in deck!");
		}else{
			Console.WriteLine("Bye!");
		}
	}
}



class Program{
	public static void Main(){
		BlackJack b = new BlackJack();
		b.Game();
	}
}