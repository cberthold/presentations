import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface WithdrawalState {
    isLoading: boolean;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface SubmitWithdrawalAction {
    type: 'SUBMIT_WITHDRAWAL';
    accountId: string;
    withdrawalAmount: number;
}

interface SubmitWithdrawalSuccessAction {
    type: 'WITHDRAWAL_SUCCESS';
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = SubmitWithdrawalAction | SubmitWithdrawalSuccessAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    submitWithdrawal: (accountId: string, withdrawalAmount: number): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();

        if (appState && appState.withdrawal && appState.withdrawal.isLoading) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }

        var parsedWithdrawalAmount = parseFloat(parseFloat(withdrawalAmount.toString()).toFixed(2));

        const data = {
            amount: parsedWithdrawalAmount,
        };

        const url = `api/Account/${accountId}/Withdrawal`;
        
        const response = await fetch(url, {
            method: "POST", // *GET, POST, PUT, DELETE, etc.
            mode: "cors", // no-cors, cors, *same-origin
            cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
            credentials: "same-origin", // include, *same-origin, omit
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data), // body data type must match "Content-Type" header
        });


        dispatch({ type: 'WITHDRAWAL_SUCCESS' });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: WithdrawalState = { isLoading: false };

export const reducer: Reducer<WithdrawalState> = (state: WithdrawalState | undefined, incomingAction: Action): WithdrawalState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'SUBMIT_WITHDRAWAL':
            return {
                isLoading: true
            };
        case 'WITHDRAWAL_SUCCESS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                isLoading: false,
            };
        default:
            return state;
    }
};
