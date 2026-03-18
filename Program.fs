open System
let tryParseInt (s: string) =
    match Int32.TryParse(s) with
    | (true, value) -> Some value
    | _ -> None

let rec readInt prompt =
    printf "%s" prompt
    let input = Console.ReadLine()
    match tryParseInt input with
    | Some value -> value
    | None ->
        printfn "Ошибка: введите целое число."
        readInt prompt

let rec readIntSequence () =

    seq {
        printf "число: "
        let input = Console.ReadLine()
        
        if input <> "" then
            match tryParseInt input with
            | Some value ->
                yield value
                yield! readIntSequence ()
            | None ->
                printfn "Ошибка: введите целое число."
                yield! readIntSequence ()
    }

let firstDigit (n: int) =
    let absN = abs n
    let rec loop x =
        if x < 10 then x
        else loop (x / 10)
    loop absN

// Задание 1
let searchFirst (numbers: seq<int>) =

    let numbersCache = numbers |> Seq.cache
    let firstDigits = numbersCache |> Seq.map firstDigit
    let numbersList = numbersCache |> Seq.toList
    let firstDigitsList = firstDigits |> Seq.toList
    printfn "Исходная последовательность :"
    numbersList |> List.iter (printf "%d ")
    printfn ""
    printfn "Последовательность первых цифр:"
    firstDigitsList |> List.iter (printf "%d ")
    printfn ""
    numbersCache

// Задание 2
let counting (numbers: seq<int>) =
    let target = readInt "Введите число для подсчёта: "
    let count =
        numbers
        |> Seq.fold (fun acc x -> 
            if x = target then
                acc + 1 
            else 
                acc) 0
    
    printfn "число %d встречается %d раз(а)" target count
    numbers

// Точка входа
[<EntryPoint>]
let main argv =
    printfn "Введите числа (пустая строчка выход):"
    let numbers = readIntSequence ()
    let numbersCache = numbers |> Seq.cache
    let numbersAfterSearch = searchFirst numbersCache
    printfn "Подсчёт вхождений"
    counting numbersCache |> ignore
    0