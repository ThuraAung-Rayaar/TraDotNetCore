namespace DotNetTraining.MinimalAPI
{
    public static class Dev_Extention
    {

       

            public static string ToJson(this object obj)
            {

                return JsonConvert.SerializeObject(obj, Formatting.Indented);

            }

            public static T? TOClass<T>(this string obj)
            {
                return JsonConvert.DeserializeObject<T>(obj);
                // return JsonConvert.SerializeObject(obj, Formatting.Indented);

            }



        

    }
}
