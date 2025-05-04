using Microsoft.Extensions.Configuration;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Providers;

public interface ILlmProvider {
    IAsyncEnumerable<string> Query(string prompt);
}

public class LlmProvider : OllamaProviderBase, ILlmProvider {
    private string _llmModel;

    public LlmProvider(IConfiguration configuration)
        : base(configuration) { }

    protected override Task Initialize() {
        _llmModel = Configuration["AppSettings:Ollama:LlmModel"];

        return base.Initialize();
    }

    public async IAsyncEnumerable<string> Query(string prompt) {
        var request = new GenerateRequest() {
            Model = _llmModel,
            Prompt = prompt,
            //Template = LlModel.Gemma3PromptTemplate
        };

        await foreach(var response in base.OllamaApiClient.GenerateAsync(request)) {
            yield return response.Response;
        }
    }
}


public static class LlModel {
    // https://ai.google.dev/gemma/docs/core/prompt-structure
    public static string Gemma3PromptTemplate => """
        <start_of_turn>user
        - Use the following notes to suggest the best response to the user query.
        - Notes are enclosed between <note> and </note> tags.
        - If there aren't notes that provide related info to the user query, return "No relevant notes found".
        - At the end of the response, include a list of all note id's used in the response, if any.
        - The output format is text plain, no markdown or json.
        {notes}
        Query: {user_prompt}<end_of_turn>
        <start_of_turn>model
        """;

    public static string NotePromptTemplate => """
        <note>
        {content}
        Id: {note_id}
        Updated: {date_updated}
        Created: {date_created}
        </note>
        """;
}