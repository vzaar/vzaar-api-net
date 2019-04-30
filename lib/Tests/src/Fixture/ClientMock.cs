using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VzaarApi;

namespace tests
{
	internal enum MockResponse
	{
		MissingData,
		DataNotArray,
		MissingMeta,
		MissingLinks,
		MissingNext,
		MissingPrevious,
		MissingLast,
		MissingFirst,
		MissingId,
		RecordPaginate,
		RecordPaginateEmpty,
		Recipe,
		RecipesList,
		PresetsList,
		Preset,
		CategoriesList,
		Category,
		Video,
		Signature,
		SignatureFailed,
		VideosList,
		UploadFailed,
		Playlist,
		PlaylistsList
	}

	internal class ClientMock : Client
	{
		internal MockResponse responseType;
		public ClientMock(MockResponse type)
		{
			responseType = type;
		}

		internal override Task<string> HttpSendAsync(HttpRequestMessage msg)
		{
			var response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Headers.Add("X-RateLimit-Limit", "222");
			response.Headers.Add("X-RateLimit-Remaining", "122");
			response.Headers.Add("X-RateLimit-Reset", "33333");

			httpHeaders = response.Headers;

			var uri = msg.RequestUri.AbsoluteUri;

			if (uri.Contains("/videos") && msg.Method == HttpMethod.Post)
				responseType = MockResponse.Video;

			if (uri.Contains("/signature") && msg.Method == HttpMethod.Post)
			{
				if (responseType != MockResponse.SignatureFailed)
					responseType = MockResponse.Signature;
			}

			var content = @"{}";

			switch (responseType)
			{
				case MockResponse.MissingData:
					content = missing_data;
					break;
				case MockResponse.DataNotArray:
					content = data_not_array;
					break;
				case MockResponse.MissingMeta:
					content = missing_meta;
					break;
				case MockResponse.MissingLinks:
					content = missing_links;
					break;
				case MockResponse.MissingFirst:
					content = missing_first;
					break;
				case MockResponse.MissingLast:
					content = missing_last;
					break;
				case MockResponse.MissingNext:
					content = missing_next;
					break;
				case MockResponse.MissingPrevious:
					content = missing_previous;
					break;
				case MockResponse.RecordPaginate:
					content = record_paginate;
					break;
				case MockResponse.RecordPaginateEmpty:
					content = record_paginate_empty;
					break;
				case MockResponse.MissingId:
					content = missing_id;
					break;
				case MockResponse.Recipe:
					content = recipe;
					break;
				case MockResponse.RecipesList:
					content = recipesList_base;
					if (msg.RequestUri.Query != "")
						content = recipesList_query;
					break;
				case MockResponse.Preset:
					content = preset;
					break;
				case MockResponse.PresetsList:
					content = presetsList_base;
					if (msg.RequestUri.Query != "")
						content = presetsList_query;
					break;
				case MockResponse.CategoriesList:
					content = categoriesList_base;
					if (msg.RequestUri.Query != "")
						content = categoriesList_query;
					break;
				case MockResponse.Category:
					content = category;
					if (msg.RequestUri.AbsoluteUri.Contains("subtree"))
						content = categoriesList_base;
					break;
				case MockResponse.Video:
					content = video;
					if (msg.RequestUri.AbsoluteUri.Contains("subtitles/26548") && (msg.Method == HttpMethod.Post || msg.Method == new HttpMethod("PATCH")))
					{
						content = subtitle_update;
					}
					else if (msg.RequestUri.AbsoluteUri.Contains("subtitles/26548") && msg.Method == HttpMethod.Delete)
					{
						content = "";
					}
					else if (msg.RequestUri.AbsoluteUri.Contains("subtitles") && msg.Method == HttpMethod.Get)
					{
						content = subtitles_list;
					}
					else if (msg.RequestUri.AbsoluteUri.Contains("subtitles"))
					{
						content = subtitle;
					}
					break;
				case MockResponse.Signature:
					content = "";
					if (msg.RequestUri.AbsolutePath.Equals("/api/v2/signature/single/2"))
						content = signature_single;

					if (msg.RequestUri.AbsolutePath.Equals("/api/v2/signature/multipart/2"))
						content = signature_multipart;

					break;
				case MockResponse.SignatureFailed:
					content = "";
					if (msg.RequestUri.AbsolutePath.Equals("/v2/signature/single/2"))
						content = signature_single_failed;

					if (msg.RequestUri.AbsolutePath.Equals("/v2/signature/multipart/2"))
						content = signature_multipart_failed;
					break;
				case MockResponse.VideosList:
					content = videosList_base;
					if (msg.RequestUri.Query != "")
						content = videosList_query;
					break;
				case MockResponse.Playlist:
					content = playlist;
					break;
				case MockResponse.PlaylistsList:
					content = playlistsList_base;
					if (msg.RequestUri.Query != "")
						content = playlistsList_query;
					break;
			}

			return Task.FromResult(content);
		}

		string record_paginate = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                },
				{
                    ""id"": 2,
                    ""name"": ""My recipe 2"",
                }
                ],
				""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""next"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""previous"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
                    	}
                	}
				}";

		string record_paginate_empty = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                },
				{
                    ""id"": 2,
                    ""name"": ""My recipe 2"",
                }
                ],
				""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""first"": null,
                        ""next"": null,
                        ""previous"": null,
                        ""last"": null
                    	}
                	}
				}";

		string missing_last = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                },
				{
                    ""id"": 2,
                    ""name"": ""My recipe 2"",
                }
                ],
				""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""next"": null,
                        ""previous"": null
                    	}
                	}
				}";

		string missing_previous = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                },
				{
                    ""id"": 2,
                    ""name"": ""My recipe 2"",
                }
                ],
				""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""next"": null,
                        ""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
                    	}
                	}
				}";

		string missing_next = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                },
				{
                    ""id"": 2,
                    ""name"": ""My recipe 2"",
                }
                ],
				""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""previous"": null,
                        ""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
                    	}
                	}
				}";

		string missing_first = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                },
				{
                    ""id"": 2,
                    ""name"": ""My recipe 2"",
                }
                ],
				""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""next"": null,
                        ""previous"": null,
                        ""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
                    	}
                	}
				}";

		string missing_links = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                },
				{
                    ""id"": 2,
                    ""name"": ""My recipe 2"",
                }
                ],
				""meta"": {
                    ""total_count"": 2,
                    ""asdf"": {
                        ""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""next"": null,
                        ""previous"": null,
                        ""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
                    	}
                	}
				}";

		string data_not_array = @"{
                ""data"": 
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                },
				""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""next"": null,
                        ""previous"": null,
                        ""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
                    	}
                	}
				}";

		string missing_data = @"""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""next"": null,
                        ""previous"": null,
                        ""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
                    }
                }";

		string missing_meta = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                },
				{
                    ""id"": 2,
                    ""name"": ""My recipe 2"",
                }
                ]
            }";

		string missing_id = @"{'data':{ 'name': 'Ingest recipe 1'}}";

		string recipe = @"{'data':{'id':1, 'name': 'Ingest recipe 1', 'multipass': false}}";

		string recipesList_base = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                    ""recipe_type"": ""new_video"",
                    ""description"": ""Test"",
                    ""account_id"": 79357,
                    ""user_id"": 79357,
                    ""default"": true,
                    ""multipass"": false,
                    ""frame_grab_time"": ""3.5"",
                    ""generate_animated_thumb"": true,
                    ""generate_sprite"": true,
                    ""use_watermark"": true,
                    ""send_to_youtube"": false,
                    ""encoding_presets"": [
                    {
                        ""id"": 2,
                        ""name"": ""Do Not Encode"",
                        ""description"": """",
                        ""output_format"": ""mp4"",
                        ""bitrate_kbps"": null,
                        ""max_bitrate_kbps"": null,
                        ""long_dimension"": null,
                        ""video_codec"": null,
                        ""profile"": ""MP3"",
                        ""frame_rate_upper_threshold"": null,
                        ""audio_bitrate_kbps"": null,
                        ""audio_channels"": null,
                        ""audio_sample_rate"": null,
                        ""created_at"": ""2016-11-09T11:01:38.000Z"",
                        ""updated_at"": ""2016-11-09T11:01:38.000Z""
                    }
                    ],
                    ""created_at"": ""2016-11-09T11:01:38.000Z"",
                    ""updated_at"": ""2016-11-25T13:30:41.000Z""
                },
				{
                    ""id"": 2,
                    ""name"": ""My recipe 2"",
                    ""recipe_type"": ""new_video"",
                    ""description"": ""Test"",
                    ""account_id"": 79357,
                    ""user_id"": 79357,
                    ""default"": true,
                    ""multipass"": false,
                    ""frame_grab_time"": ""3.5"",
                    ""generate_animated_thumb"": true,
                    ""generate_sprite"": true,
                    ""use_watermark"": true,
                    ""send_to_youtube"": false,
                    ""encoding_presets"": [
                    {
                        ""id"": 2,
                        ""name"": ""Do Not Encode"",
                        ""description"": """",
                        ""output_format"": ""mp4"",
                        ""bitrate_kbps"": null,
                        ""max_bitrate_kbps"": null,
                        ""long_dimension"": null,
                        ""video_codec"": null,
                        ""profile"": ""MP3"",
                        ""frame_rate_upper_threshold"": null,
                        ""audio_bitrate_kbps"": null,
                        ""audio_channels"": null,
                        ""audio_sample_rate"": null,
                        ""created_at"": ""2016-11-09T11:01:38.000Z"",
                        ""updated_at"": ""2016-11-09T11:01:38.000Z""
                    }
                    ],
                    ""created_at"": ""2016-11-09T11:01:38.000Z"",
                    ""updated_at"": ""2016-11-25T13:30:41.000Z""
                }
                ],
                ""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=2"",
                        ""next"": null,
                        ""previous"": null,
                        ""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
                    }
                }
            }";

		string recipesList_query = @"{
                ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""My recipe"",
                    ""recipe_type"": ""new_video"",
                    ""description"": ""Test"",
                    ""account_id"": 79357,
                    ""user_id"": 79357,
                    ""default"": true,
                    ""multipass"": false,
                    ""frame_grab_time"": ""3.5"",
                    ""generate_animated_thumb"": true,
                    ""generate_sprite"": true,
                    ""use_watermark"": true,
                    ""send_to_youtube"": false,
                    ""encoding_presets"": [
                    {
                        ""id"": 2,
                        ""name"": ""Do Not Encode"",
                        ""description"": """",
                        ""output_format"": ""mp4"",
                        ""bitrate_kbps"": null,
                        ""max_bitrate_kbps"": null,
                        ""long_dimension"": null,
                        ""video_codec"": null,
                        ""profile"": ""MP3"",
                        ""frame_rate_upper_threshold"": null,
                        ""audio_bitrate_kbps"": null,
                        ""audio_channels"": null,
                        ""audio_sample_rate"": null,
                        ""created_at"": ""2016-11-09T11:01:38.000Z"",
                        ""updated_at"": ""2016-11-09T11:01:38.000Z""
                    }
                    ],
                    ""created_at"": ""2016-11-09T11:01:38.000Z"",
                    ""updated_at"": ""2016-11-25T13:30:41.000Z""
                }
                ],
                ""meta"": {
                    ""total_count"": 2,
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=1"",
                        ""next"": null,
                        ""previous"": null,
                        ""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
                    }
                }
            }";

		string presetsList_base = @"{
			  ""data"": [
			    {
			      ""id"": 4,
			      ""name"": ""LD"",
			      ""description"": ""Low Definition"",
			      ""output_format"": ""mp4"",
			      ""bitrate_kbps"": 400,
			      ""max_bitrate_kbps"": 520,
			      ""long_dimension"": 480,
			      ""video_codec"": ""libx264"",
			      ""profile"": ""main"",
			      ""frame_rate_upper_threshold"": ""29.97"",
			      ""audio_bitrate_kbps"": 128,
			      ""audio_channels"": 2,
			      ""audio_sample_rate"": 44100,
			      ""created_at"": ""2016-10-24T12:36:47.000Z"",
			      ""updated_at"": ""2016-10-24T12:36:47.000Z""
			    },
			    {
			      ""id"": 5,
			      ""name"": ""SD - Lower"",
			      ""description"": ""Our lower Standard Definition rendition"",
			      ""output_format"": ""mp4"",
			      ""bitrate_kbps"": 800,
			      ""max_bitrate_kbps"": 1040,
			      ""long_dimension"": 640,
			      ""video_codec"": ""libx264"",
			      ""profile"": ""main"",
			      ""frame_rate_upper_threshold"": ""29.97"",
			      ""audio_bitrate_kbps"": 128,
			      ""audio_channels"": 2,
			      ""audio_sample_rate"": 44100,
			      ""created_at"": ""2016-10-24T12:36:47.000Z"",
			      ""updated_at"": ""2016-10-24T12:36:47.000Z""
			    }
			  ],
			  ""meta"": {
			    ""total_count"": 2,
			    ""links"": {
			      ""first"": ""http://api.vzaar.com/api/v2/encoding_presets?page=1"",
			      ""next"": null,
			      ""previous"": ""http://api.vzaar.com/api/v2/encoding_presets?page=1"",
			      ""last"": ""http://api.vzaar.com/api/v2/encoding_presets?page=5""
			    }
			  }
			}";

		string presetsList_query = @"{
			  ""data"": [
			    {
			      ""id"": 4,
			      ""name"": ""LD"",
			      ""description"": ""Low Definition"",
			      ""output_format"": ""mp4"",
			      ""bitrate_kbps"": 400,
			      ""max_bitrate_kbps"": 520,
			      ""long_dimension"": 480,
			      ""video_codec"": ""libx264"",
			      ""profile"": ""main"",
			      ""frame_rate_upper_threshold"": ""29.97"",
			      ""audio_bitrate_kbps"": 128,
			      ""audio_channels"": 2,
			      ""audio_sample_rate"": 44100,
			      ""created_at"": ""2016-10-24T12:36:47.000Z"",
			      ""updated_at"": ""2016-10-24T12:36:47.000Z""
			    },
			  ],
			  ""meta"": {
			    ""total_count"": 2,
			    ""links"": {
			      ""first"": ""http://api.vzaar.com/api/v2/encoding_presets?page=1"",
			      ""next"": null,
			      ""previous"": ""http://api.vzaar.com/api/v2/encoding_presets?page=1"",
			      ""last"": ""http://api.vzaar.com/api/v2/encoding_presets?page=5""
			    }
			  }
			}";

		string preset = @"{
                ""data"": {
                    ""id"": 3,
                    ""name"": ""ULD"",
                    ""description"": ""Ultra Low Definition"",
                    ""output_format"": ""mp4"",
                    ""bitrate_kbps"": 200,
                    ""max_bitrate_kbps"": 260,
                    ""long_dimension"": 416,
                    ""video_codec"": ""libx264"",
                    ""profile"": ""main"",
                    ""frame_rate_upper_threshold"": ""12.0"",
                    ""audio_bitrate_kbps"": 128,
                    ""audio_channels"": 2,
                    ""audio_sample_rate"": 44100,
                    ""created_at"": ""2016-10-24T12:36:47.000Z"",
                    ""updated_at"": ""2016-10-24T12:36:47.000Z""
                }
            }";

		string categoriesList_base = @"{
                ""data"": [
                {
                    ""id"": 42,
                    ""account_id"": 1,
                    ""user_id"": 1,
                    ""name"": ""Biology"",
                    ""description"": null,
                    ""parent_id"": 1,
                    ""depth"": 0,
                    ""node_children_count"": 3,
                    ""tree_children_count"": 5,
                    ""node_video_count"": 3,
                    ""tree_video_count"": 6,
                    ""created_at"": ""2015-04-06T22:03:24.000Z"",
                    ""updated_at"": ""2016-01-06T12:08:38.000Z""
                },
                {
                    ""id"": 2,
                    ""account_id"": 1,
                    ""user_id"": 1,
                    ""name"": ""Chemistry"",
                    ""description"": null,
                    ""parent_id"": 42,
                    ""depth"": 0,
                    ""node_children_count"": 3,
                    ""tree_children_count"": 5,
                    ""node_video_count"": 3,
                    ""tree_video_count"": 6,
                    ""created_at"": ""2015-04-06T22:03:24.000Z"",
                    ""updated_at"": ""2016-01-06T12:08:38.000Z""
                }
                ],
                ""meta"": {
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/categories/42/subtree?page=1"",
                        ""last"": ""http://api.vzaar.com/api/v2/categories/42/subtree?page=4"",
                        ""next"": null,
                        ""previous"": null
                    },
                    ""total_count"": 2
                }
            }";

		string categoriesList_query = @"{
                ""data"": [
                {
                    ""id"": 2,
                    ""account_id"": 1,
                    ""user_id"": 1,
                    ""name"": ""Chemistry"",
                    ""description"": null,
                    ""parent_id"": 42,
                    ""depth"": 0,
                    ""node_children_count"": 3,
                    ""tree_children_count"": 5,
                    ""node_video_count"": 3,
                    ""tree_video_count"": 6,
                    ""created_at"": ""2015-04-06T22:03:24.000Z"",
                    ""updated_at"": ""2016-01-06T12:08:38.000Z""
                }
                ],
                ""meta"": {
                    ""links"": {
                        ""first"": ""http://api.vzaar.com/api/v2/categories/42/subtree?page=1"",
                        ""last"": ""http://api.vzaar.com/api/v2/categories/42/subtree?page=4"",
                        ""next"": null,
                        ""previous"": null
                    },
                    ""total_count"": 2
                }
            }";

		string category = @"{
                ""data"": {
                    ""id"": 1,
                    ""account_id"": 1,
                    ""user_id"": 1,
                    ""name"": ""Sciences"",
                    ""description"": null,
                    ""parent_id"": null,
                    ""depth"": 0,
                    ""node_children_count"": 3,
                    ""tree_children_count"": 5,
                    ""node_video_count"": 3,
                    ""tree_video_count"": 6,
                    ""created_at"": ""2015-04-06T22:03:24.000Z"",
                    ""updated_at"": ""2016-01-06T12:08:38.000Z""
                }
            }";

		string video = @"{
                ""data"": {
                    ""id"": 7574853,
                    ""title"": ""multipart"",
                    ""user_id"": 42,
                    ""account_id"": 1,
                    ""description"": null,
                    ""duration"": 66.7,
                    ""created_at"": ""2016-11-11T11:36:26.000Z"",
                    ""updated_at"": ""2016-11-11T11:37:36.000Z"",
                    ""private"": false,
                    ""seo_url"": ""http://example.com/video.mp4"",
                    ""url"": null,
					""poster_url"": ""https://view.vzaar.com/123/image"",
                    ""state"": ""ready"",
                    ""thumbnail_url"": ""https://view.vzaar.com/7574853/thumb"",
                    ""embed_code"": ""<iframe id=\""vzvd-7574853\"" name=\""vzvd-7574853\"" title=\""video player\"" class=\""video-player\"" type=\""text/html\"" width=\""448\"" height=\""278\"" frameborder=\""0\"" allowfullscreen allowTransparency=\""true\"" mozallowfullscreen webkitAllowFullScreen src=\""//view.vzaar.com/7574853/player\""></iframe>"",
                    ""renditions"": [
                    {
                        ""id"": 66,
                        ""width"": 416,
                        ""height"": 258,
                        ""bitrate"": 200,
                        ""framerate"": ""12.0"",
                        ""status"": ""finished"",
                        ""size_in_bytes"": 12345
                    }
                    ],
                    ""legacy_renditions"": [
                    {
                        ""id"": 10567122,
                        ""type"": ""standard"",
                        ""width"": 448,
                        ""height"": 278,
                        ""bitrate"": 512,
                        ""status"": ""Finished""
                    }
                    ],
					""subtitles"": [
					{
						""id"": 26548,
						""language"": ""English"",
						""code"": ""en"",
						""title"": ""9991021-en.srt"",
						""url"": ""https://view.vzaar.com/subtitles/26548"",
						""created_at"": ""2018-11-27T14:53:24.120Z"",
						""updated_at"": ""2018-11-27T14:53:24.120Z""
					}
					]
                }
            }";

		string signature_single = @"{
                ""data"": {
					""x-amz-credential"": ""AKIAJ74MFWNVAFH6P7FQ/20181101/us-east-1/s3/aws4_request"",
					""x-amz-algorithm"": ""AWS4-HMAC-SHA256"",
					""x-amz-date"": ""20181101T151558Z"",
					""x-amz-signature"": ""<signature-string>"",
                    ""key"": ""vzaar/vz9/1e8/source/vz91e80db09a494467b265f0c327950825/${filename}"",
                    ""acl"": ""private"",
                    ""policy"": ""<signed-policy-string>"",
                    ""success_action_status"": ""201"",
                    ""content_type"": ""binary/octet-stream"",
                    ""guid"": ""vz91e80db09a494467b265f0c327950825"",
                    ""bucket"": ""vzaar-upload-development"",
                    ""upload_hostname"": ""https://vzaar-upload-development.s3.amazonaws.com""
                }
            }";

		string signature_multipart = @"{
                ""data"": {
					""x-amz-credential"": ""AKIAJ74MFWNVAFH6P7FQ/20181101/us-east-1/s3/aws4_request"",
					""x-amz-algorithm"": ""AWS4-HMAC-SHA256"",
					""x-amz-date"": ""20181101T151558Z"",
					""x-amz-signature"": ""<signature-string>"",
                    ""key"": ""vzaar/vz9/1e8/source/vz91e80db09a494467b265f0c327950825/${filename}"",
                    ""acl"": ""private"",
                    ""policy"": ""<signed-policy-string>"",
                    ""success_action_status"": ""201"",
                    ""content_type"": ""binary/octet-stream"",
                    ""guid"": ""vz91e80db09a494467b265f0c327950825"",
                    ""bucket"": ""vzaar-upload-development"",
                    ""upload_hostname"": ""https://vzaar-upload-development.s3.amazonaws.com"",
                    ""part_size"": ""16mb"",
                    ""part_size_in_bytes"": 16777216,
                    ""parts"": 4
                }
            }";

		string signature_single_failed = @"{
                ""data"": {
					""x-amz-credential"": ""AKIAJ74MFWNVAFH6P7FQ/20181101/us-east-1/s3/aws4_request"",
					""x-amz-algorithm"": ""AWS4-HMAC-SHA256"",
					""x-amz-date"": ""20181101T151558Z"",
					""x-amz-signature"": ""<signature-string>"",
                    ""key"": null
                    ""acl"": ""private"",
                    ""policy"": ""<signed-policy-string>"",
                    ""success_action_status"": ""201"",
                    ""content_type"": ""binary/octet-stream"",
                    ""guid"": ""vz91e80db09a494467b265f0c327950825"",
                    ""bucket"": ""vzaar-upload-development"",
                    ""upload_hostname"": ""https://vzaar-upload-development.s3.amazonaws.com""
                }
            }";

		string signature_multipart_failed = @"{
                ""data"": {
					""x-amz-credential"": ""AKIAJ74MFWNVAFH6P7FQ/20181101/us-east-1/s3/aws4_request"",
					""x-amz-algorithm"": ""AWS4-HMAC-SHA256"",
					""x-amz-date"": ""20181101T151558Z"",
					""x-amz-signature"": ""<signature-string>"",
                    ""key"": ""vzaar/vz9/1e8/source/vz91e80db09a494467b265f0c327950825/${filename}"",
                    ""acl"": ""private"",
                    ""policy"": ""<signed-policy-string>"",
                    ""success_action_status"": ""201"",
                    ""content_type"": ""binary/octet-stream"",
                    ""guid"": ""vz91e80db09a494467b265f0c327950825"",
                    ""bucket"": ""vzaar-upload-development"",
                    ""upload_hostname"": ""https://vzaar-upload-development.s3.amazonaws.com"",
                    ""part_size"": ""16mb"",
                    ""part_size_in_bytes"": null,
                    ""parts"": 4
                }
            }";

		string videosList_base = @"{
				  ""data"": [
				    {
				      ""id"": 7574853,
				      ""title"": ""multipart"",
				      ""user_id"": 42,
				      ""account_id"": 1,
				      ""description"": null,
				      ""duration"": 66.7,
				      ""created_at"": ""2016-11-11T11:36:26.000Z"",
				      ""updated_at"": ""2016-11-11T11:37:36.000Z"",
				      ""private"": false,
				      ""seo_url"": ""http://example.com/video.mp4"",
				      ""url"": null,
				      ""state"": ""ready"",
				      ""thumbnail_url"": ""https://view.vzaar.com/42/thumb"",
				      ""embed_code"": ""<iframe id=\""vzvd-42\"" name=\""vzvd-7574853\"" title=\""video player\"" class=\""video-player\"" type=\""text/html\"" width=\""448\"" height=\""278\"" frameborder=\""0\"" allowfullscreen allowTransparency=\""true\"" mozallowfullscreen webkitAllowFullScreen src=\""//view.vzaar.com/42/player\""></iframe>"",
				      ""renditions"": [
				        {
				          ""id"": 66,
				          ""width"": 416,
				          ""height"": 258,
				          ""bitrate"": 200,
				          ""framerate"": ""12.0"",
				          ""status"": ""finished"",
				          ""size_in_bytes"": 12345
				        }
				      ],
				      ""legacy_renditions"": [
				        {
				          ""id"": 10567122,
				          ""type"": ""standard"",
				          ""width"": 448,
				          ""height"": 278,
				          ""bitrate"": 512,
				          ""status"": ""Finished""
				        }
				      ]
				    },
					{
				      ""id"": 7574856,
				      ""title"": ""multipart"",
				      ""user_id"": 42,
				      ""account_id"": 1,
				      ""description"": null,
				      ""duration"": 66.7,
				      ""created_at"": ""2016-11-11T11:36:26.000Z"",
				      ""updated_at"": ""2016-11-11T11:37:36.000Z"",
				      ""private"": false,
				      ""seo_url"": ""http://example.com/video.mp4"",
				      ""url"": null,
				      ""state"": ""ready"",
				      ""thumbnail_url"": ""https://view.vzaar.com/42/thumb"",
				      ""embed_code"": ""<iframe id=\""vzvd-42\"" name=\""vzvd-7574853\"" title=\""video player\"" class=\""video-player\"" type=\""text/html\"" width=\""448\"" height=\""278\"" frameborder=\""0\"" allowfullscreen allowTransparency=\""true\"" mozallowfullscreen webkitAllowFullScreen src=\""//view.vzaar.com/42/player\""></iframe>"",
				      ""renditions"": [
				        {
				          ""id"": 66,
				          ""width"": 416,
				          ""height"": 258,
				          ""bitrate"": 200,
				          ""framerate"": ""12.0"",
				          ""status"": ""finished"",
				          ""size_in_bytes"": 12345
				        }
				      ],
				      ""legacy_renditions"": [
				        {
				          ""id"": 10567122,
				          ""type"": ""standard"",
				          ""width"": 448,
				          ""height"": 278,
				          ""bitrate"": 512,
				          ""status"": ""Finished""
				        }
				      ]
				    }
				  ],
				  ""meta"": {
				    ""total_count"": 212,
				    ""links"": {
				      ""first"": ""https://api.vzaar.com/api/v2/videos?order=asc&page=1&state=ready"",
				      ""next"": null,
				      ""previous"": null,
				      ""last"": ""https://api.vzaar.com/api/v2/videos?order=asc&page=9&state=ready""
				    }
				  }
				}";

		string videosList_query = @"{
				  ""data"": [
					{
				      ""id"": 7574856,
				      ""title"": ""multipart"",
				      ""user_id"": 42,
				      ""account_id"": 1,
				      ""description"": null,
				      ""duration"": 66.7,
				      ""created_at"": ""2016-11-11T11:36:26.000Z"",
				      ""updated_at"": ""2016-11-11T11:37:36.000Z"",
				      ""private"": false,
				      ""seo_url"": ""http://example.com/video.mp4"",
				      ""url"": null,
				      ""state"": ""ready"",
				      ""thumbnail_url"": ""https://view.vzaar.com/42/thumb"",
				      ""embed_code"": ""<iframe id=\""vzvd-42\"" name=\""vzvd-7574853\"" title=\""video player\"" class=\""video-player\"" type=\""text/html\"" width=\""448\"" height=\""278\"" frameborder=\""0\"" allowfullscreen allowTransparency=\""true\"" mozallowfullscreen webkitAllowFullScreen src=\""//view.vzaar.com/42/player\""></iframe>"",
				      ""renditions"": [
				        {
				          ""id"": 66,
				          ""width"": 416,
				          ""height"": 258,
				          ""bitrate"": 200,
				          ""framerate"": ""12.0"",
				          ""status"": ""finished"",
				          ""size_in_bytes"": 12345
				        }
				      ],
				      ""legacy_renditions"": [
				        {
				          ""id"": 10567122,
				          ""type"": ""standard"",
				          ""width"": 448,
				          ""height"": 278,
				          ""bitrate"": 512,
				          ""status"": ""Finished""
				        }
				      ]
				    }
				  ],
				  ""meta"": {
				    ""total_count"": 212,
				    ""links"": {
				      ""first"": ""https://api.vzaar.com/api/v2/videos?order=asc&page=1&state=ready"",
				      ""next"": null,
				      ""previous"": null,
				      ""last"": ""https://api.vzaar.com/api/v2/videos?order=asc&page=9&state=ready""
				    }
				  }
				}";

		string playlist = @"{
			  ""data"": {
			    ""id"": 1,
			    ""category_id"": 42,
			    ""title"": ""test"",
			    ""sort_order"": ""desc"",
			    ""sort_by"": ""created_at"",
			    ""max_vids"": 43,
			    ""position"": ""right"",
			    ""private"": false,
			    ""dimensions"": ""768x340"",
			    ""autoplay"": false,
			    ""continuous_play"": true,
			    ""created_at"": ""2017-03-20T11:30:36.932Z"",
			    ""updated_at"": ""2017-03-20T11:30:36.932Z""
			  }
			}";

		string playlistsList_base = @"{
			  ""data"": [
			    {
			      ""id"": 1,
					""title"": ""drj-playlist-cat-user-test"",
					""sort_order"": ""asc"",
			      ""sort_by"": ""created_at"",
			      ""max_vids"": 30,
			      ""position"": ""left"",
			      ""private"": false,
			      ""dimensions"": ""auto"",
			      ""autoplay"": false,
			      ""continuous_play"": false,
			      ""category_id"": 42,
			      ""created_at"": ""2016-11-09T11:01:38.000Z"",
			      ""updated_at"": ""2016-11-25T13:30:41.000Z""
			    },
				{
			      ""id"": 2,
					  ""title"": ""drj-playlist-cat-user-test"",
					  ""sort_order"": ""asc"",
			      ""sort_by"": ""created_at"",
			      ""max_vids"": 30,
			      ""position"": ""left"",
			      ""private"": false,
			      ""dimensions"": ""auto"",
			      ""autoplay"": false,
			      ""continuous_play"": false,
			      ""category_id"": 42,
			      ""created_at"": ""2016-11-09T11:01:38.000Z"",
			      ""updated_at"": ""2016-11-25T13:30:41.000Z""
			    }
			  ],
			  ""meta"": {
			    ""total_count"": 2,
			    ""links"": {
			      ""first"": ""http://api.vzaar.com/api/v2/feeds/playlists?page=1&per_page=1"",
			      ""next"": null,
			      ""previous"": null,
			      ""last"": ""http://api.vzaar.com/api/v2/feeds/playlists?page=2&per_page=1""
			    }
			  }
			 }";

		string playlistsList_query = @"{
			  ""data"": [
			    {
			      ""id"": 1,
					  ""title"": ""drj-playlist-cat-user-test"",
					  ""sort_order"": ""asc"",
			      ""sort_by"": ""created_at"",
			      ""max_vids"": 30,
			      ""position"": ""left"",
			      ""private"": false,
			      ""dimensions"": ""auto"",
			      ""autoplay"": false,
			      ""continuous_play"": false,
			      ""category_id"": 42,
			      ""created_at"": ""2016-11-09T11:01:38.000Z"",
			      ""updated_at"": ""2016-11-25T13:30:41.000Z""
			    }
			  ],
			  ""meta"": {
			    ""total_count"": 2,
			    ""links"": {
			      ""first"": ""http://api.vzaar.com/api/v2/feeds/playlists?page=1&per_page=1"",
			      ""next"": null,
			      ""previous"": null,
			      ""last"": ""http://api.vzaar.com/api/v2/feeds/playlists?page=2&per_page=1""
			    }
			  }
			}";

		string subtitle = @"{
			  ""data"": {
			    ""id"": 26548,
			    ""language"": ""English"",
			    ""code"": ""en"",
			    ""title"": ""9991021-en.srt"",
			    ""url"": ""https://view.vzaar.com/subtitles/26548"",
			    ""created_at"": ""2018-11-27T14:53:24.120Z"",
			    ""updated_at"": ""2018-11-27T14:53:24.120Z""
			  }
			}";

		string subtitle_update = @"{
			  ""data"": {
			    ""id"": 26548,
			    ""language"": ""English"",
			    ""code"": ""en"",
			    ""title"": ""9991021-en.srt"",
			    ""url"": ""https://view.vzaar.com/subtitles/26548"",
			    ""created_at"": ""2018-11-27T14:53:24.120Z"",
			    ""updated_at"": ""2019-11-27T14:53:24.120Z""
			  }
			}";

		string subtitles_list = @"{
				""data"": [
					{
						""id"": 26319,
						""language"": ""Polish"",
						""code"": ""pl"",
						""title"": ""9991021-pl.srt"",
						""url"": ""https://view.vzaar.com/subtitles/26319"",
						""created_at"": ""2018-11-24T19:01:57.000Z"",
						""updated_at"": ""2018-11-24T19:01:58.000Z""
					}
				],
				""meta"": {
					""total_count"": 1,
					""links"": {
						""first"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=1&per_page=1"",
						""next"": null,
						""previous"": null,
						""last"": ""http://api.vzaar.com/api/v2/ingest_recipes?page=2&per_page=1""
					}
				}
			}";

	}//end class
}//end namespace

