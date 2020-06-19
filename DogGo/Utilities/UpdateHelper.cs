using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Utilities
{
    public class UpdateHelper

        //If the field is Nul
    { 
        public static void SetNullableString(SqlCommand cmd, string parameter, string value)
        {

            
            if (value == null)
            {
                //allowing the input field to be null/creating a null value if the input field is left blank
                cmd.Parameters.AddWithValue(parameter, DBNull.Value);
            }
            else
            {
                //sending the value of what is put in the input field
                cmd.Parameters.AddWithValue(parameter, value);
            }
        }

    }
}
