open System
open LLama.Common
open LLama

type ModelParam = {
    modelPath : string
    contextSize : int
    seed : int
    gpuLayerCount : int
}
let modelPath = "../../models/wizardLM-7B.ggmlv3.q8_0.bin"

let modelParam = 
    { 
        modelPath = modelPath
        contextSize = 1024
        seed = 1337
        gpuLayerCount = 5
    }

[<EntryPoint>]
let main argv =
    let modelParams = new Common.ModelParams(modelParam.modelPath, modelParam.contextSize)
    let model = new LLamaModel(modelParams)
    let mutable prompt = "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.\r\n\r\nUser: Hello, Bob.\r\nBob: Hello. How may I help you today?\r\nUser: Please tell me the largest city in Europe.\r\nBob: Sure. The largest city in Europe is Moscow, the capital of Russia.\r\nUser:"; // use the "chat-with-bob" prompt here.
    let ex = new InteractiveExecutor(model)
    let session = new ChatSession(ex)
    while not (prompt = "stop") do
        let inferenceParams = new InferenceParams()
        inferenceParams.Temperature <- 0.6f
        inferenceParams.AntiPrompts <- ["User:"]
        let text = session.Chat(prompt, inferenceParams)
        text 
        |> Seq.map(fun x -> Console.Write(x))
        |> ignore

        prompt <- Console.ReadLine()

    session.SaveSession("SavedSession")
    0