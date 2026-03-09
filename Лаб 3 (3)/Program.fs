open System
open System.IO
// Функция для корректной проверки ввода
let rec promptForPath() =
    printf "Введите путь к директории: "
    let input = Console.ReadLine()
    let raw = if isNull input 
              then "" 
                else input.Trim()
    let path =
        if  raw.StartsWith("\"") &&
            raw.EndsWith("\"") &&
            raw.Length >= 2 
        then
            raw.Substring(1, raw.Length - 2)
            else
                raw

    if String.IsNullOrWhiteSpace path then
        eprintfn "Ошибка: путь не указан!"
        promptForPath()
    elif not (Directory.Exists path) then
        eprintfn "Ошибка: Директория '%s' не существует!" path
        printfn "Попробуйте снова."
        promptForPath()
    else
        path

let getDirectory (argv: string[]) =
    if argv.Length > 0 
    then
        let arg = argv.[0]
        if Directory.Exists arg 
        then
            arg
            else
            eprintfn "Ошибка: Директория '%s' не существует!" 
                arg
            promptForPath()
    else
        promptForPath()

// Для поиска .txt файлов
let rec findTxtFiles dir =
    seq {
        let files = Directory.GetFiles(dir, "*.txt")
        for file in files do
            yield file

        let subDirs = Directory.GetDirectories(dir)
        for subDir in subDirs do
            yield! findTxtFiles subDir
    }

let printResults results =
    if Seq.isEmpty results 
    then
        printfn "Файлы .txt не найдены."
        else
            results |> Seq.iter (printfn "%s")

[<EntryPoint>]
let main argv =

    let directory = getDirectory argv
    let fullPath = Path.GetFullPath directory
    printfn "Поиск файлов .txt в '%s' и его подкаталогах:" 
        fullPath
    findTxtFiles fullPath
    |> printResults

    0