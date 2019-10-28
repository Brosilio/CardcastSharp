using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace CardcastSharp
{
    /// <summary>
    /// The base class for fetching Cardcast decks
    /// </summary>
    public static class Cardcast
    {
        /// <summary>
        /// The API endpoint for Cardcast
        /// </summary>
        public const string API_ENDPOINT = "https://api.cardcastgame.com/v1/decks/";

        /// <summary>
        /// The URL for cards
        /// </summary>
        public const string API_CARDS = "/cards";

        /// <summary>
        /// The WebClient used internally to get stuff.
        /// </summary>
        private static readonly WebClient webClient = new WebClient();

        /// <summary>
        /// Returns a deck of cards by code. Will throw exception if something goes wrong.
        /// </summary>
        /// <param name="code">The Cardcast deck play code</param>
        public static async Task<Deck> GetDeck(string code)
        {
            string deckJson = await webClient.DownloadStringTaskAsync($"{API_ENDPOINT}{code}{API_CARDS}");
            Deck temp = JsonConvert.DeserializeObject<Deck>(deckJson);
            temp.Info = await GetDeckInformation(code);
            return temp;
        }

        /// <summary>
        /// Returns all the information about a deck. Will throw exception if something goes wrong.
        /// </summary>
        /// <param name="code">The playcode of the deck.</param>
        public static async Task<DeckInfo> GetDeckInformation(string code)
        {
            string deckInfoJson = await webClient.DownloadStringTaskAsync($"{API_ENDPOINT}{code}");
            return JsonConvert.DeserializeObject<DeckInfo>(deckInfoJson);
        }
    }

    /// <summary>
    /// A helper class for caching decks in memory.
    /// Create a new one of these with the required information, then use GetDeck on it.
    /// It will either try to get the deck from the server if it isn't cached, or return
    /// the cached version.
    /// </summary>
    public class CardcastCache
    {
        /// <summary>
        /// Helper class for stuff that needs to be done.
        /// </summary>
        private class DeckCacheObject
        {
            public DateTime cacheTime;
            public Deck cachedDeck;

            public DeckCacheObject(Deck deck)
            {
                this.cacheTime = DateTime.Now;
                this.cachedDeck = deck;
            }
        }

        private readonly Dictionary<string, DeckCacheObject> cachedDecks = new Dictionary<string, DeckCacheObject>();

        /// <summary>
        /// The time-to-live for cached decks, in seconds. If they've gone foul, they will be redownloaded.
        /// </summary>
        public float TTL { get; set; }

        /// <summary>
        /// New Cardcast Cache.
        /// </summary>
        /// <param name="ttl">The time to live of cached decks, in seconds.</param>
        public CardcastCache(float ttl)
        {
            this.TTL = ttl;
        }

        /// <summary>
        /// Gets a deck using this CardcastCache's cache (if it has it.)
        /// Otherwise, it downloads it, then caches it.
        /// Will throw if angered or provoked.
        /// </summary>
        /// <param name="playCode">The playcode of the deck to get.</param>
        /// <returns></returns>
        public async Task<Deck> GetDeck(string playCode)
        {
            playCode = playCode.ToLower().Trim();

            if (this.cachedDecks.ContainsKey(playCode))
            {
                if (DateTime.Now.Subtract(this.cachedDecks[playCode].cacheTime).TotalSeconds < this.TTL)
                {
                    return this.cachedDecks[playCode].cachedDeck;
                }
                else
                {
                    this.cachedDecks[playCode] = new DeckCacheObject(await Cardcast.GetDeck(playCode));
                    return this.cachedDecks[playCode].cachedDeck;
                }
            }
            else
            {
                this.cachedDecks.Add(playCode, new DeckCacheObject(await Cardcast.GetDeck(playCode)));
                return this.cachedDecks[playCode].cachedDeck;
            }
        }

        /// <summary>
        /// Expunges a cached deck from the cached decks.
        /// </summary>
        /// <param name="playCode">The playcode of the deck to expunge.</param>
        /// <returns>Returns true if the deck was expunged, false if not.</returns>
        public bool Expunge(string playCode)
        {
            playCode = playCode.ToLower().Trim();
            if (this.cachedDecks.ContainsKey(playCode))
            {
                this.cachedDecks.Remove(playCode);
                return true;
            }

            return false;
        }
    }



    /// <summary>
    /// Contains information about decks.
    /// </summary>
    public class DeckInfo
    {
        /// <summary>
        /// Information about the author of the deck
        /// </summary>
        public class DeckAuthor
        {
            /// <summary>
            /// The ID of the author
            /// </summary>
            [JsonProperty("id")]
            public string ID { get; private set; }

            /// <summary>
            /// The username of the author
            /// </summary>
            [JsonProperty("username")]
            public string Username { get; private set; }
        }

        /// <summary>
        /// The name of the deck
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// The play code of the deck
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; private set; }

        /// <summary>
        /// The description of the deck
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        /// The date on which the deck was created
        /// </summary>
        [JsonProperty("created_at")]
        public string CreationTime { get; private set; }

        /// <summary>
        /// The time at which this deck was last modified
        /// </summary>
        [JsonProperty("updated_at")]
        public string ModificationTime { get; private set; }

        /// <summary>
        /// Associated copyright information URL
        /// </summary>
        [JsonProperty("copyright_holder_url")]
        public string CopyrightHolderURL { get; private set; }

        /// <summary>
        /// The category
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; private set; }

        /// <summary>
        /// True if this deck is unlisted (private)
        /// </summary>
        [JsonProperty("unlisted")]
        public bool Unlisted { get; private set; }

        /// <summary>
        /// True if the deck has an external copyright holder
        /// </summary>
        [JsonProperty("external_copyright")]
        public bool ExternalCopyright { get; private set; }

        /// <summary>
        /// The total number of call cards (black cards)
        /// </summary>
        [JsonProperty("call_count")]
        public int CallCount { get; private set; }

        /// <summary>
        /// The total number of response cards (white cards)
        /// </summary>
        [JsonProperty("response_count")]
        public int ResponseCount { get; private set; }

        /// <summary>
        /// The rating of this deck
        /// </summary>
        [JsonProperty("rating")]
        public float Rating { get; private set; }

        /// <summary>
        /// The author of this deck
        /// </summary>
        [JsonProperty("author")]
        public DeckAuthor Author { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// The deck class contains all the calls and responses for the deck
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// All the information about this deck.
        /// </summary>
        [JsonIgnore]
        public DeckInfo Info { get; set; }

        /// <summary>
        /// All the calls (black cards) in this deck
        /// </summary>
        [JsonProperty("calls")]
        public Card[] Calls { get; set; }

        /// <summary>
        /// All the responses (white cards) in this deck
        /// </summary>
        [JsonProperty("responses")]
        public Card[] Responses { get; set; }

        public Deck() { }

        /// <summary>
        /// Create a new deck and initialize it with set calls and responses
        /// </summary>
        /// <param name="calls"></param>
        /// <param name="responses"></param>
        public Deck(Card[] calls, Card[] responses)
        {
            this.Calls = calls;
            this.Responses = responses;
        }

        /// <summary>
        /// Returns the deck as a JSON object
        /// </summary>
        /// <returns>This deck as a JSON object</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// The base card class
    /// </summary>
    public class Card
    {
        /// <summary>
        /// The ID of this card
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// The card itself
        /// </summary>
        [JsonProperty("text")]
        public string[] Text { get; set; }

        /// <summary>
        /// The date and time at which this card was created
        /// </summary>
        [JsonProperty("created_at")]
        public string CreationTime { get; set; }

        /// <summary>
        /// True if this card is NSFW
        /// </summary>
        [JsonProperty("nsfw")]
        public bool IsNSFW { get; set; }

        /// <summary>
        /// This returns the card as a JSON object
        /// </summary>
        /// <returns>The card as JSON</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
