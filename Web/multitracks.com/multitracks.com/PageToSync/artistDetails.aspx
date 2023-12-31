﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="artistDetails.aspx.cs" Inherits="artistDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no" />
    <meta charset="utf-8"/>
    <title>MultiTracks.com</title>
    <link media="all" rel="stylesheet" href="./css/index.css"/>
    
</head>
<body>
    <form id="form1" runat="server">
        <noscript>
            <div>Javascript must be enabled for the correct page display</div>
        </noscript>

        <a class="accessibility" href="#main" tabindex="21">Skip to Content</a>

        <div class="wrapper mod-standard mod-gray">
            <div class="details-banner">
                <div class="details-banner--overlay"></div>
                <div class="details-banner--hero">
                    <asp:Literal ID="litHeroImage" runat="server"></asp:Literal>
                </div>
                <div class="details-banner--info">
                    <a href="#" class="details-banner--info--box">
                        <asp:Literal ID="litImage" runat="server"></asp:Literal>
                    </a>
                    <h1 class="details-banner--info--name">
                        <asp:HyperLink ID="artistName" CssClass="details-banner--info--name--link" Text="Bethel Music" runat="server"></asp:HyperLink>
                    </h1>
                </div>
            </div>

            <nav class="discovery--nav">
                <ul class="discovery--nav--list tab-filter--list u-no-scrollbar">
                    <li class="discovery--nav--list--item tab-filter--item is-active">
                        <a class="tab-filter" href="../artists/details.aspx">Overview</a>
                    </li>
                    <li class="discovery--nav--list--item tab-filter--item">
                        <a class="tab-filter" href="../artists/songs/details.aspx">Songs</a>
                    </li>
                    <li class="discovery--nav--list--item tab-filter--item">
                        <a class="tab-filter" href="../artists/albums/details.aspx">Albums</a>
                    </li>
                </ul>
            </nav>

            <div class="discovery--container u-container">
                <main class="discovery--section">
                    <section class="standard--holder">
                        <div class="discovery--section--header">
                            <h2>Top Songs</h2>
                            <a class="discovery--section--header--view-all" href="#">View All</a>
                        </div>

                        <asp:Repeater ID="songRepeater" runat="server">
                            <ItemTemplate>
                                <li>
                                    <%# Eval("SongInfo") %>
                                </li>  
                            </ItemTemplate>
                        </asp:Repeater>
                    </section>

                    <div class="discovery--space-saver">
                        <section class="standard--holder">
                            <div class="discovery--section--header">
                                <h2>Albums</h2>
                                <a class="discovery--section--header--view-all" href="/artists/default.aspx">View All</a>
                            </div>

                            <div class="album-container">
        <asp:Repeater ID="albumsRepeater" runat="server">
            <ItemTemplate>
                <div class="media-item">
                    <a class="media-item--img--link" href="#" tabindex="0">
                        <img class="media-item--img" alt='<%# Eval("title") %>' src='<%# ResolveUrl(Eval("imageURL").ToString()) %>' srcset='<%# ResolveUrl(Eval("imageURL").ToString()) %>, <%# ResolveUrl(Eval("imageURL").ToString()) %> 2x' style='max-width:200px;max-height:200px'/>
                        <span class="image-tag"></span>
                    </a>
                    <a class="media-item--title" href="#" tabindex="0"><%# Eval("title") %></a>
                    <a class="media-item--subtitle" href="#" tabindex="0"><%# Eval("ArtistName") %></a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
                        </section>
                    </div>

                    <section class="standard--holder">
                        <div class="discovery--section--header">
                            <h2>Biography</h2>
                        </div>

                        <div class="artist-details--biography biography">
                            <asp:Literal ID="litBiography" runat="server"></asp:Literal>
                            <a href="#">Read More...</a>
                        </div>
                    </section>
                </main>
            </div>
        </div>

        <a class="accessibility" href="#wrapper" tabindex="20">Back to top</a>
    </form>
</body>
</html>