# CardcastSharp
CardcastSharp is a C# api wrapper for the Cardcastgame.com API.

## Releases
If you're lazy and don't want to bother reading the docs, head over to the [releases](https://github.com/Brosilio/CardcastSharp/releases) page.

## Getting Started
The Cardcast API is simple by nature. This wrapper is equally simple.

Don't forget to
```csharp
using CardcastSharp;
```

To get a deck:
```csharp
Deck d = await Cardcast.GetDeck("4KU68", OnGetDeckError);
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

Deck d = await Cardcast.GetDeck("4KU68", OnGetDeckError);
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
```

Getting information for a deck:

```csharp
Deck d = await Cardcast.GetDeck("4KU68", OnGetDeckError);
if(d != null)
{
	d.Info // Contains lots of information about the deck
}


// Alternate method

DeckInfo di = await Cardcast.GetDeckInfo("4KU68", OnGetDeckError);
if(di != null)
{
	di.Author // Lots of properties in here
}

```

## License
Just don't be a dick.

Neither FEAR Labs or myself owns Cardcast. This is simply a wrapper for their cool thing.