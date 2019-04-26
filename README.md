# CardcastSharp
CardcastSharp is a C# api wrapper for the Cardcastgame.com API.

## Releases
If you're lazy and don't want to bother reading the docs, head over to the [####releases](https://github.com/Brosilio/CardcastSharp/releases) page.

## Getting Started
The Cardcast API is simple by nature. This wrapper is equally simple.

To get a deck:
```csharp
Deck d = await Cardcast.GetDeck(Console.ReadLine(), OnGetDeckError);
if(d != null)
{
	// Do stuff
}

private void OnGetDeckError(Exception error)
{
	// Handle error here
}
```
Passing a callback to the GetDeck() function allows some shit to be done if something fucks up internally.

Getting the cards in the deck:
```csharp

Deck d = await Cardcast.GetDeck(Console.ReadLine(), OnGetDeckError);
if(d != null)
{
	foreach(Card c in d.Calls)
	{
		// Do something with all the calls (black cards) in the deck
	}

	foreach(Card c in d.Responses)
	{
		// Do something with all the responses (white cards) in the deck
	}
}

## License
Just don't be a dick.

Neither FEAR Labs or myself owns Cardcast. This is simply a wrapper for their cool thing.