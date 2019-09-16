using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class Subtitle
	{

		internal Record record;

		//constructor
		internal Subtitle (long videoId, Client client)
		{
			record = new Record ("videos/" + videoId.ToString() + "/subtitles", client);
		}

		internal Subtitle (long videoId, Client client, long subtitleId)
		{
			record = new Record ("videos/" + videoId.ToString() + "/subtitles", client);
			/*
            The endpoint does not provide Lookup, thus
            it is not possible to read data to 
            initialize the object before it is used.
            */
			record ["id"] = subtitleId;
		}

		internal Subtitle (Record item)
		{
			record = item;
		}

		public object this[string index]{

			get { return record [index];}

		}

		public object ToTypeDef(Type type){

			return record.ToTypeDef (type);

		}

		//ASYNC methods

		//create
		internal async Task CreateAsync(Dictionary<string,object> tokens) {

			if(tokens.ContainsKey("file")) {

				var filepath = tokens["file"].ToString();

				await record.CreateAsync (tokens, null, filepath).ConfigureAwait(false);

			} else {

				await record.CreateAsync (tokens).ConfigureAwait(false);
				
			}
		}

		//update
		internal async Task SaveAsync(Dictionary<string,object> tokens) {

			if(tokens.ContainsKey("file")) {

				var filepath = tokens["file"].ToString();

				await record.UpdateAsync (tokens, null, filepath).ConfigureAwait(false);

			} else {
				
				await record.UpdateAsync (tokens).ConfigureAwait(false);

			}
		}

		//delete
		internal async Task DeleteAsync() {

			await record.DeleteAsync ().ConfigureAwait(false);

		}

		// synchronous methods

		//create
		internal void Create(Dictionary<string,object> tokens) {
			CreateAsync (tokens).Wait ();
		}

		//update
		internal void Save(Dictionary<string,object> tokens) {
			SaveAsync (tokens).Wait ();
		}

		//delete
		internal void Delete() {
			DeleteAsync ().Wait ();
		}

	}//end class
}//end namespace