module Register

  open FSharp.Data

  open CommonLibrary
  open PublicService.Backoffice

  [<Literal>]
  let sampleFile = "Import/Register/batch_20190111_sarah.txt"

  type DataForSubsidieRegister =
    CsvProvider<Sample      = sampleFile,
                Separators  = "|",
                HasHeaders  = true,
                Encoding    = "utf-8"
                >

  type Counter = { Total: int; Successes: int }

  let getRows (batchFile:string) =
    let publicServices = DataForSubsidieRegister.Load(batchFile)
    publicServices.Rows

  let importRow counter apiBaseUrl token rowCount cursorTop (row:DataForSubsidieRegister.Row) =

    let result =
      postNewPublicService apiBaseUrl token row.Naam

    let counter =
      match result with
      | Success _ -> { Total = counter.Total + 1; Successes = counter.Successes + 1 }
      | Failure _ -> { Total = counter.Total + 1; Successes = counter.Successes }

    System.Console.CursorTop <- cursorTop
    printfn ""
    printfn "\rProcessing %i/%i ..." counter.Total rowCount
    printfn "\rSuccesses: %i/%i ..." counter.Successes rowCount
    printfn "\rErrors: %i/%i ..." (counter.Total - counter.Successes) rowCount

    (row.Id |> string, result), counter

  let importRows apiBaseUrl token rows =
    let rowCount = Seq.length rows
    let cursorTop = System.Console.CursorTop
    Seq.mapFold (fun i row -> importRow i apiBaseUrl token rowCount cursorTop row) { Total = 0; Successes = 0 } rows
    |> fun (x, _) -> x
    |> Seq.toList
