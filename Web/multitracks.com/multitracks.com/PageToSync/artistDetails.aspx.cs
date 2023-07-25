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

        // If no artistID is found in the query string, set it to 1 (default artist ID)
        artistID = 1;
        return true;
    }


    // Load artist details using the provided ID
    private void LoadArtistDetails(int artistID)
    {
        try
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
            else
            {
                // Handle the case when no artist details are found for the given ID
                Response.Write("<script>alert('Artist details not found for the provided ID. Loading default artist ID.');</script>");
               
                    // Load artist details using the ID
                    LoadArtistDetails(1);

                
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur
            HandleError(ex);
        }
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
        
    }