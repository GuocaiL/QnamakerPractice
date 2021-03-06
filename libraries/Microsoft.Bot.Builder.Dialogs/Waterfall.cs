﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace Microsoft.Bot.Builder.Dialogs
{
    /// <summary>
    /// Dialog optimized for prompting a user with a series of questions. Waterfalls accept a stack of
    /// functions which will be executed in sequence.Each waterfall step can ask a question of the user
    /// and the users response will be passed as an argument to the next waterfall step.
    /// </summary>
    public class Waterfall : Dialog, IDialogContinue, IDialogResume
    {
        private WaterfallStep[] _steps;

        public Waterfall(WaterfallStep[] steps)
        {
            _steps = steps;
        }

        public Task DialogBeginAsync(DialogContext dc, IDictionary<string, object> dialogArgs = null)
        {
            if (dc == null)
            {
                throw new ArgumentNullException(nameof(dc));
            }

            dc.ActiveDialog.Step = 0;
            return RunStepAsync(dc, dialogArgs);
        }

        public async Task DialogContinueAsync(DialogContext dc)
        {
            if (dc == null)
            {
                throw new ArgumentNullException(nameof(dc));
            }

            if (dc.Context.Activity.Type == ActivityTypes.Message)
            {
                dc.ActiveDialog.Step++;
                await RunStepAsync(dc, new Dictionary<string, object> { { "Activity", dc.Context.Activity } }).ConfigureAwait(false);
            }
        }

        public Task DialogResumeAsync(DialogContext dc, IDictionary<string, object> result)
        {
            if (dc == null)
            {
                throw new ArgumentNullException(nameof(dc));
            }

            dc.ActiveDialog.Step++;
            return RunStepAsync(dc, result);
        }

        private async Task RunStepAsync(DialogContext dc, IDictionary<string, object> result = null)
        {
            if (dc == null)
            {
                throw new ArgumentNullException(nameof(dc));
            }

            var step = dc.ActiveDialog.Step;
            if (step >= 0 && step < _steps.Length)
            {
                SkipStepFunction next = (r) =>
                {
                    // Skip to next step
                    dc.ActiveDialog.Step++;
                    return RunStepAsync(dc, r);
                };

                // Execute step
                await _steps[step](dc, result, next).ConfigureAwait(false);
            }
            else
            {
                // End of waterfall so just return to parent
                await dc.EndAsync(result).ConfigureAwait(false);
            }
        }
    }
}
