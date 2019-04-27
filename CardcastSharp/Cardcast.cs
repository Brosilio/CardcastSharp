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

        private static WebClient webClient = new WebClient();

        /// <summary>
        /// Returns a deck of cards by code
        /// </summary>
        /// <param name="code">The Cardcast deck play code</param>
        /// <param name="onErrorCallback">Call this method if there is some error while obtaining the deck</param>
        /// <returns>Returns null if the deck couldn't be found or there was some fucking shit error while trying to get it</returns>
        public static async Task<Deck> GetDeck(string code, Action<Exception> onErrorCallback = null)
        {
            try
            {
                string deckJson = await webClient.DownloadStringTaskAsync($"{API_ENDPOINT}{code}{API_CARDS}");
                Deck temp = JsonConvert.DeserializeObject<Deck>(deckJson);
                temp.Info = await GetDeckInformation(code, onErrorCallback);
                return temp;
            }
            catch(Exception ex)
            {
                onErrorCallback?.Invoke(ex);
                return null;
            }
        }

        /// <summary>
        /// Returns all the information about a deck.
        /// </summary>
        /// <param name="code">The playcode of the deck.</param>
        /// <returns>Returns the deck info. Returns null if it couldn't be found.</returns>
        public static async Task<DeckInfo> GetDeckInformation(string code, Action<Exception> onErrorCallback = null)
        {
            try
            {
                string deckInfoJson = await webClient.DownloadStringTaskAsync($"{API_ENDPOINT}{code}");
                return JsonConvert.DeserializeObject<DeckInfo>(deckInfoJson);
            }
            catch (Exception ex)
            {
                onErrorCallback?.Invoke(ex);
                return null;
            }
        }
    }

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
