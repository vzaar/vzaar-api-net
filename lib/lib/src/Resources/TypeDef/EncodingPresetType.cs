using System;

namespace VzaarApi
{
	/*The class is defined to be used with ToTypeDef method*/

	public class EncodingPresetType
	{
		public long? id;
		public string name;
		public string description;
		public string output_format;
		public long? bitrate_kbps;
		public long? max_bitrate_kbps;
		public long? long_dimension;
		public string video_codec;
		public string profile;
		public string frame_rate_upper_threshold;
		public long? audio_bitrate_kbps;
		public long? audio_channels;
		public long? audio_sample_rate;
		public DateTime created_at;
		public DateTime updated_at;
	}
}

