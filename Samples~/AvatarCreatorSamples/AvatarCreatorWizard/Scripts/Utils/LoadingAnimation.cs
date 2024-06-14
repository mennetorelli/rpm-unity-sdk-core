using System;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarCreator;
using UnityEngine;

namespace ReadyPlayerMe.Samples.AvatarCreatorWizard
{
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private Transform[] circles;
        [SerializeField] private float minScaleFactor = 0.1f;

        private CancellationTokenSource ctx;

        private async void OnEnable()
        {
            ctx = new CancellationTokenSource();
            foreach (var circle in circles)
            {
                circle.localScale = Vector3.one * minScaleFactor;
            }

            while (!ctx.IsCancellationRequested)
            {
                foreach (var circle in circles)
                {
                    try
                    {
                        await circle.LerpScale(Vector3.one * 1f, 0.1f, ctx.Token);
                        await Task.Delay(TimeSpan.FromSeconds(0.1), ctx.Token);
                        await circle.LerpScale(Vector3.one * minScaleFactor, 0.2f, ctx.Token);
                    }
                    catch
                    {
                        // ignored
                    }

                }
            }
        }

        private void OnDestroy()
        {
            ctx.Dispose();
        }

        private void OnDisable()
        {
            ctx.Cancel();
        }
    }
}
