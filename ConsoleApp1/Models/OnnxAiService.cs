using KBIPMobileBackend.Services;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OnnxAiService : IChatService, IDisposable
{
    private readonly InferenceSession _session;

    public OnnxAiService(string modelPath)
    {
        // Загружаем ONNX-модель
        _session = new InferenceSession(modelPath);
    }

    // Реализация интерфейса
    public Task<string> AskAsync(string prompt)
    {
        // Можно запустить синхронный метод в Task.Run, чтобы не блокировать вызывающий поток
        return Task.Run(() => AskInternal(prompt));
    }

    // Вспомогательный синхронный метод
    private string AskInternal(string prompt)
    {
        // TODO: здесь ваша логика токенизации prompt в inputIds и attentionMask
        var inputIds = new long[] { /* … */ };
        var attentionMask = new long[] { /* … */ };

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_ids",
                new DenseTensor<long>(inputIds, new[] { 1, inputIds.Length })),
            NamedOnnxValue.CreateFromTensor("attention_mask",
                new DenseTensor<long>(attentionMask, new[] { 1, attentionMask.Length }))
        };

        using var results = _session.Run(inputs);

        // Предположим, что выходной тензор называется "logits"
        var logits = results.First(x => x.Name == "logits").AsTensor<float>();
        int vocabSize = logits.Dimensions[2];
        // Берём логиты последнего токена
        float[] lastLogits = new float[vocabSize];
        int seqLen = logits.Dimensions[1];
        for (int i = 0; i < vocabSize; i++)
            lastLogits[i] = logits[0, seqLen - 1, i];

        int nextToken = ArgMax(lastLogits);
        string generated = TokenToString(nextToken);
        return generated;
    }

    private int ArgMax(float[] arr)
    {
        int idx = 0;
        float max = arr[0];
        for (int i = 1; i < arr.Length; i++)
            if (arr[i] > max)
            {
                max = arr[i];
                idx = i;
            }
        return idx;
    }

    private string TokenToString(int token)
    {
        if (BasicVocabulary.TokenToWord.TryGetValue(token, out var word))
            return word;
        return "<unk>";
    }


    public void Dispose() => _session.Dispose();
}
