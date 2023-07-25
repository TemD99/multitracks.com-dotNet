using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using DataAccess;



    public partial class artistDetails : System.Web.UI.Page
    {
        // List of Songs
        public List<Song> Songs { get; set; }

        // Executes when the page is loaded
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Only executes if it's not a post back
                if (!IsPostBack)
                {
                    LoadDefault();

                    // Check if the artist ID in the query string is valid
                    if (HasValidArtistId(out int artistID))
                    {
                        // Load artist details using the ID
                        LoadArtistDetails(artistID);
                   
                }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur
                HandleError(ex);
            }
        }

        // Check if the artist ID in the query string is valid
        private bool HasValidArtistId(out int artistID)
        {
            // Try to get the artist ID from the query string
            string artistIDString = Request.QueryString["artistID"];
            if (!string.IsNullOrEmpty(artistIDString) && int.TryParse(artistIDString, out artistID))
            {
                return true;
            }

            // Display an alert if the artist ID is invalid
            Response.Write("<script>alert('Please Enter a valid artistID.');</script>");
            artistID = 0;
            return false;
        }

    

    // Load artist details using the provided ID
    private void LoadArtistDetails(int artistID)
    {
        // Create a new SQL data access object
        SQL dataAccess = new SQL();

        // Create a parameter for the stored procedure
        SqlParameter parameter = new SqlParameter("@ArtistId", artistID);
        dataAccess.Parameters.Add(parameter);

        // Execute the stored procedure and get the result
        DataSet artistDetailsAndSongs = dataAccess.ExecuteStoredProcedureDS("GetArtistDetails");
        dataAccess.Parameters.Clear();

        // Check if any artist details were returned
        if (artistDetailsAndSongs.Tables[0].Rows.Count > 0)
        {
            // Get the artist information from the first row
            GetArtistInfo(artistDetailsAndSongs.Tables[0].Rows[0]);
        }

        // Check if any songs were returned
        if (artistDetailsAndSongs.Tables.Count > 1)
        {
            // Get the songs and bind them to the repeater
            GetSongs(artistDetailsAndSongs.Tables[1], artistDetailsAndSongs.Tables[2]);
            BindSongs();
        }

        // Check if any albums were returned
        if (artistDetailsAndSongs.Tables.Count > 2)
        {
            // Bind albums to the repeater
            albumsRepeater.DataSource = artistDetailsAndSongs.Tables[2];
            albumsRepeater.DataBind();
        }
    }

    // Bind the songs to the repeater
    private void BindSongs()
        {
            songRepeater.DataSource = Songs;
            songRepeater.DataBind();
        }

        // Get the songs from the provided DataTable
        private void GetSongs(DataTable songsTable, DataTable albumTable)
        {
            Songs = songsTable.AsEnumerable().Select(songRow => GetSong(songRow, albumTable)).ToList();
        }

        // Get a Song object from a DataRow
        private Song GetSong(DataRow songRow, DataTable albumTable)
        {
            Song song = new Song();
            string albumId = songRow["albumID"].ToString();
            DataRow albumRow = albumTable.AsEnumerable().FirstOrDefault(row => row["albumID"].ToString() == albumId);
            string albumName = albumRow != null ? albumRow["title"].ToString() : "Unknown Album";
            song.SongInfo = $"{songRow["title"].ToString()} - {albumName} - {songRow["bpm"].ToString()} - {songRow["timeSignature"].ToString()}";
            return song;
        }

        // Handle any exceptions that occur
        private void HandleError(Exception ex)
        {
            // Log the exception and display an alert
            System.Diagnostics.Trace.WriteLine($"An error occurred: {ex.ToString()}");
            Response.Write("<script>alert('An error occurred while loading the artist details. Please try again later.');</script>");
        }

        // Get artist information from a DataRow
        private void GetArtistInfo(DataRow row)
        {
            // Set the artist's banner image
            SetArtistBanner(Convert.ToString(row["heroURL"]));
            // Set the artist's image
            SetArtistImage(Convert.ToString(row["imageURL"]));
            // Set the artist's name
            artistName.Text = Convert.ToString(row["title"]);
            // Set the artist's biography
            litBiography.Text = GetFormattedBiography(Convert.ToString(row["biography"]));
        }

        // Set the artist's banner image
        private void SetArtistBanner(string artistHeroImage)
        {
            litHeroImage.Text = $"<img class='details-banner--hero--img' src='{ResolveUrl(artistHeroImage)}' srcset='{ResolveUrl(artistHeroImage)}, {ResolveUrl(artistHeroImage)} 2x' alt='Bethel Music' />";
        }

        // Set the artist's image
        private void SetArtistImage(string artistImage)
        {
            litImage.Text = $"<img class='details-banner--info--box--img' src='{ResolveUrl(artistImage)}' alt='Artist image' />";
        }

        // Get the artist's biography with HTML formatting
        private string GetFormattedBiography(string biography)
        {
            return $"<p>{biography.Replace("\n", "<br/>")}</p>";
        }

        // Load the default values
        private void LoadDefault()
        {
            // Set the default banner image
            SetArtistBanner("./img/31.jpg");
            // Set the default artist image
            SetArtistImage("./img/174.jpg");
        // Set the default biography
        string bio = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>" +

"<p>\"Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?\"</p>" +

"<p></p>";
        litBiography.Text = GetFormattedBiography(bio);

            // Set the default songs
            Songs = new List<Song>()
        {
            new Song { SongInfo = "<span class='song-title'>Default Song 1</span> - <span class='song-bpm'>120</span> - <span class='song-time-signature'>4/4</span>"},
            new Song { SongInfo = "<span class='song-title'>Default Song 2</span> - <span class='song-bpm'>100</span> - <span class='song-time-signature'>3/4</span>"},
            new Song { SongInfo = "<span class='song-title'>Default Song 3</span> - <span class='song-bpm'>140</span> - <span class='song-time-signature'>4/4</span>"}
        };

            // Bind the default songs to the repeater
            BindSongs();
        }
    }