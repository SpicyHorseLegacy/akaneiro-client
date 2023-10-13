/// <summary>
/// This class handles user events. Anything connected to the player, such as age, country, gender, etc.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA_User
{
	public enum Gender { Unknown, Male, Female }
	
	#region public methods
	
	public void NewUser(Gender gender, int? birth_year, int? friend_count)
	{
		CreateNewUser(gender, birth_year, friend_count);
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Adds information about the user/player
	/// </summary>
	/// <param name="gender">
	/// The gender of the user. If the gender is unknown information will not be submitted.
	/// </param>
	/// <param name="birth_year">
	/// The year the user was born. Set to "null" if unknown.
	/// </param>
	/// <param name="country">
	/// The ISO2 country code the user is playing from. See: http://en.wikipedia.org/wiki/ISO_3166-2. Set to "null" if unknown.
	/// </param>
	/// <param name="state">
	/// The code of the country state the user is playing from. Set to "null" if unknown.
	/// </param>
	/// /// <param name="friend_count">
	/// The number of friends in the user's network. Set to "null" if unknown.
	/// </param>
	private void CreateNewUser(Gender gender, int? birth_year, int? friend_count)
	{
		Hashtable parameters = new Hashtable();
		
		if (gender == Gender.Male)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Gender], 'M');
		}
		else if (gender == Gender.Female)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Gender], 'F');
		}
		
		if (birth_year.HasValue && birth_year.Value != 0)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Birth_year], birth_year.ToString());
		}
		
		if (friend_count.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Friend_Count], friend_count.ToString());
		}
		
		if (parameters.Count == 0)
		{
			GA.LogWarning("GA: No data to send with NewUser event; event will not be added to queue");
			return;
		}

		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_User, false);
	}
	
	#endregion
}