
namespace DotNetTraining.MinimalAPI.EndPoints.Drugs
{
    public static class DrugsEndPoint
    {

        public static void UseDrugEndPoint(this IEndpointRouteBuilder app) {
            string Filepath = "Data/Drug_Sample_Info.json";

            app.MapGet("/Drugs", () => {

               
                var jsonStr = File.ReadAllText(Filepath);
                var result = jsonStr.TOClass<Drug_Sample>();

                return Results.Ok(result.Drug_Tbl.ToList());


            }).WithName("GetDrugs").WithOpenApi();

            app.MapGet("/Drugs/Drug_id={id}", (int id) => {

                string Filepath = "Data/Drug_Sample_Info.json";
                var jsonStr = File.ReadAllText(Filepath);
                var result = jsonStr.TOClass<Drug_Sample>();//.Drug_Tbl.ToList().Where(x=> x.Drug_id == id).FirstOrDefault();
                var item = result.Drug_Tbl.Where(x => x.Drug_id == id).FirstOrDefault();
                return result is null ? Results.NotFound("NO Item") : Results.Ok(item);
                // return Results.Ok(result.Drug_Tbl.ToList());


            }).WithName("GetDrugByID").WithOpenApi();

            app.MapGet("/Drugs/Drug_Name={Name}", (string Name) => {

                //string Filepath = "Data/Drug_Sample_Info.json";
                var jsonStr = File.ReadAllText(Filepath);
              // var result = JsonConvert.DeserializeObject<Drug_Sample>(jsonStr);//.Drug_Tbl.ToList().Where(x=> x.Drug_id == id).FirstOrDefault();
               var result = jsonStr.TOClass<Drug_Sample>();
                var item = result.Drug_Tbl.Where(x => x.Generic_name == Name || x.Brand_name == Name).FirstOrDefault();
                return result is null ? Results.NotFound("NO Item") : Results.Ok(item);
                // return Results.Ok(result.Drug_Tbl.ToList());


            }).WithName("GetDrugByName").WithOpenApi();

            app.MapPost("/Drugs", (Drug_Idendity sample) =>{

                var jsonDrug = File.ReadAllText(Filepath);

               var DrugArray =  jsonDrug.TOClass<Drug_Sample>();
                var DrugList = DrugArray.Drug_Tbl.ToList();
                DrugList.Add(sample);
                DrugArray.Drug_Tbl = DrugList.ToArray();

                File.WriteAllText(Filepath, DrugArray.ToJson());


                //var Druglist = jsonStr.TOClass<Drug_Sample>();
                ////var re = Druglist.Drug_Tbl.ToList().Add(sample);
                //var itemList = Druglist.Drug_Tbl.ToList();
                //itemList.Add(sample);
                //var jsonstr2 = Druglist.ToJson();
                //File.WriteAllText(Filepath, jsonstr2);
                
                return Results.Ok(DrugArray.Drug_Tbl);
            
            }).WithName("Post_New_Drug").WithOpenApi();


            app.MapPut("/Drugs/{id}", (int id,Drug_Idendity sample) => {

                var jsonDrug = File.ReadAllText(Filepath);

                var DrugArray = jsonDrug.TOClass<Drug_Sample>();
                var DrugItem = DrugArray.Drug_Tbl.ToList().Where(x=>x.Drug_id == id).FirstOrDefault();
                if (DrugItem is null) return Results.NotFound("No Item Found");
                DrugItem = sample;
                DrugItem.Drug_id = id;
                
                DrugArray.Drug_Tbl[id-1] = DrugItem;
                File.WriteAllText(Filepath, DrugArray.ToJson());
                return Results.Ok("Successful update");
            }).WithName("Update Drug").WithOpenApi();

            app.MapDelete("/Drugs/{id}", (int id) => {

                var jsonDrug = File.ReadAllText(Filepath);

                var DrugArray = jsonDrug.TOClass<Drug_Sample>();



                var table = DrugArray.Drug_Tbl ;

                Array.Copy(table, 0, DrugArray.Drug_Tbl, 0, id - 2);
               // int index = 5 - 1;
                Array.Copy(table, 0, DrugArray.Drug_Tbl, 0, id-1);
                Array.Copy(table, id, DrugArray.Drug_Tbl, id-1,  table.Length - id);

                File.WriteAllText(Filepath, DrugArray.ToJson());
                return Results.Ok("Successful Delete");
            }).WithName("Delete Drug").WithOpenApi();
        }

    }
}
