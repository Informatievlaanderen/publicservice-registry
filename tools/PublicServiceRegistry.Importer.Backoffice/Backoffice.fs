module PublicService.Backoffice

  open System
  open FSharp.Data
  open FSharp.Data.HttpRequestHeaders

  open FSharp.Data.JsonExtensions

  open CommonLibrary
  open PublicService.Csv
  open PublicService.JsonRequests
  open PublicService.ActivePatterns

  let postNewPublicService apiBaseUri token (name:string) =
    try
      let createRequest =
        NewPublicService.Create(
            naam = name
          )
          .JsonValue
          .ToString(JsonSaveOptions.DisableFormatting)

      Http.Request
        ( url = sprintf "%s/v1/dienstverleningen" apiBaseUri,
          body = HttpRequestBody.TextRequest createRequest,
          headers = [ ContentTypeWithEncoding (HttpContentTypes.Json, Text.Encoding.UTF8)
                      Authorization (sprintf "Bearer %s" token) ],
          httpMethod = HttpMethod.Post,
          silentHttpErrors = false
        )
      |> Success
    with
      | ex -> ex |> fail

  //let putPublicService apiBaseUri token (row:PublicServices.Row) isSubsidy dvrCode =
  //  try
  //    let updateRequest =
  //      UpdatePublicService
  //        .Update(
  //            id = dvrCode,
  //            naam = row.``NaamAflopend sorteren``,
  //            bevoegdeAutoriteitOvoNummer = row.``Nieuwe OVO-code verantw ent``,
  //            isSubsidie = isSubsidy
  //            )
  //        .JsonValue
  //        .ToString()

  //    Http.Request
  //      ( url = sprintf "%s/v1/dienstverleningen/%s" apiBaseUri dvrCode,
  //        body = HttpRequestBody.TextRequest updateRequest,
  //        headers = [ ContentTypeWithEncoding (HttpContentTypes.Json, Text.Encoding.UTF8)
  //                    Authorization (sprintf "Bearer %s" token) ],
  //        httpMethod = HttpMethod.Put,
  //        silentHttpErrors = false
  //      )
  //    |> Success
  //      with
  //  | ex -> ex |> fail

  let updatePublicService apiBaseUri token (name:string) (competentAuthority:string) (isSubsidy:bool) (dvrCode:string) =
    try
      let updateRequest =
        UpdatePublicService.Update(
          id = dvrCode,
          naam = name,
          bevoegdeAutoriteitOvoNummer = competentAuthority,
          isSubsidie = false)
          .JsonValue
          .ToString()

      Http.Request
        ( url = sprintf "%s/v1/dienstverleningen/%s" apiBaseUri dvrCode,
          body = HttpRequestBody.TextRequest updateRequest,
          headers = [ ContentTypeWithEncoding (HttpContentTypes.Json, Text.Encoding.UTF8)
                      Authorization (sprintf "Bearer %s" token) ],
          httpMethod = HttpMethod.Put,
          silentHttpErrors = false
        )
      |> Success
        with
    | ex -> ex |> fail

  let putAltNameIpdc apiBaseUri token (row:PublicServices.Row) altName dvrCode =
    try
      let updateRequest =
        UpdateAlternativeNameIpdc
          .UpdateAlternativeNameIpdc(
              labels = UpdateAlternativeNameIpdc.Labels(
                ipdc = altName
                )
              )
          .JsonValue
          .ToString()

      Http.Request
        ( url = sprintf "%s/v1/dienstverleningen/%s/alternatievebenamingen" apiBaseUri dvrCode,
          body = HttpRequestBody.TextRequest updateRequest,
          headers = [ ContentTypeWithEncoding (HttpContentTypes.Json, Text.Encoding.UTF8)
                      Authorization (sprintf "Bearer %s" token) ],
          httpMethod = HttpMethod.Patch,
          silentHttpErrors = false
        )
      |> Success
        with
    | ex -> ex |> fail

  let putAltNameSubsidieRegister apiBaseUri token altName dvrCode =
    try
      let updateRequest =
        UpdateAlternativeNameSubsidieRegister
          .UpdateAlternativeNameSubsidieRegister(
              labels = UpdateAlternativeNameSubsidieRegister.Labels(
                subsidieregister = altName
                )
              )
          .JsonValue
          .ToString()

      Http.Request
        ( url = sprintf "%s/v1/dienstverleningen/%s/alternatievebenamingen" apiBaseUri dvrCode,
          body = HttpRequestBody.TextRequest updateRequest,
          headers = [ ContentTypeWithEncoding (HttpContentTypes.Json, Text.Encoding.UTF8)
                      Authorization (sprintf "Bearer %s" token) ],
          httpMethod = HttpMethod.Patch,
          silentHttpErrors = false
        )
      |> Success
        with
    | ex -> ex |> fail

  let getPublicService apiBaseUri dvrCode =
    try

      let response =
        Http.Request
          ( url = sprintf "%s/v1/dienstverleningen/%s" apiBaseUri dvrCode,
            httpMethod = HttpMethod.Get,
            silentHttpErrors = false
          )

      match response.Body with
      | Text text ->
        JsonValue.Parse(text) |> Success
      | Binary _ ->
        failwith "Expected text reponse but got binary"

    with
    | ex -> ex |> fail

  let extractDvrCode (result:HttpResponse) =
    try
      let location = result.Headers.[HttpResponseHeaders.Location]
      match location with
      | DVR dvr -> Success dvr
      | _ -> failwithf "Could not find DVR code in %s" location |> fail
    with
      | ex -> ex |> fail
