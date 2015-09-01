using System.Diagnostics;
using Locafi.Client.Model.Dto.Places;
using Simple.OData.Client;

namespace Locafi.Client.Odata
{
    public class shitty
    {
        public async void Something()
        {

            
            var settings = new ODataClientSettings("http://legacynavapi.azurewebsites.net/api/");
            settings.MetadataDocument = @"http://legacynavapi.azurewebsites.net/api/$metadata";
            settings.BeforeRequest += delegate (System.Net.Http.HttpRequestMessage request)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJuYW1laWQiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAzMzMiLCJyYW1wYXV0aC90b2tlbmlkIjoiOTE1MTBiODQtMzQ3My00NmZlLThkN2QtM2UyODFlM2M1YjczIiwicmFtcGF1dGgvYXV0aFRva2VuIjoidHJ1ZSIsImdpdmVuX25hbWUiOiJSaWFuIiwidW5pcXVlX25hbWUiOiJSaWFuIEZpbm5lZ2FuIiwiZmFtaWx5X25hbWUiOiJGaW5uZWdhbiIsImVtYWlsIjoicmZpbm5lZ2FuQHJhbXAuY29tLmF1IiwicmFtcGF1dGgvY2xpZW50SWQiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAzMzMiLCJpc3MiOiJodHRwOi8vUmFtcE9ubGluZUF1dGhTZXJ2ZXIiLCJhdWQiOiIxZDNlZjUyNWI4NGQ0NTAyOGRkM2IyYjg0MzZlOGI0NyIsImV4cCI6MTQ0MDIyMzE5MSwibmJmIjoxNDQwMTM1ODkxfQ.fWdCvL-s_pS2A_qp0QtdmZlH2-PCHocoVgVUcMd4ZWGa1-w32HSoXnKoIXlg6xC-ON5Vxm0C80pMQG6U7K8eA5n6KnVRJMJOjwuYdcx8bZPzp0rwDgJigYYz3i9uAY6bFWvl8LrUAwqpJMfDxogWtMhdFCCkwk9vuEPTHQxQaLquu1xTkuYV22iB5FAMYwpD6apbFC5CNLIRAjO2vhJFCLJkCnaXSkn1ri2V0qP8vE5rm4Mt7PL5mIVrTGtuwXu3fm9Zp18L7hbrZORYM4yyAod6vqXm-bwjau_scFGty5NyP3t2TVBr2SaU8Q2YdU7jg8vVaGnReOZYs4X34NrN-Q");
            };

            

            var client = new ODataClient(settings);

            //var packages = await client
            //    .For<ItemDetailDto>()
            //    .Filter(x => x.TagNumber == "303400000800004000000003")
            //    .FindEntriesAsync();

            //foreach (var package in packages)
            //{
            //    Debug.WriteLine(package.Name);
            //}

            var c = client.For<PlaceDetailDto>()
                .Filter((place) => place.Description.Contains("e"));

            
            var com = await c.GetCommandTextAsync();
            Debug.WriteLine(com);
        }
    }
}
