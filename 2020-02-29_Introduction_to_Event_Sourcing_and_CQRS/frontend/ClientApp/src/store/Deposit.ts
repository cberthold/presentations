import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

const accountNumber = "4ff8fae5-e2fe-4d65-9f59-cf95cb5f31ea";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface DepositState {
    isLoading: boolean;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface SubmitDepositAction {
    type: 'SUBMIT_DEPOSIT';
    depositAmount: number;
}

interface SubmitDepositSuccessAction {
    type: 'DEPOSIT_SUCCESS';
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = SubmitDepositAction | SubmitDepositSuccessAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    submitDeposit: (depositAmount: number): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();

        if (appState && appState.deposit && appState.deposit.isLoading) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }

        var parsedDepositAmount = parseFloat(parseFloat(depositAmount.toString()).toFixed(2));

        const data = {
            amount: parsedDepositAmount,
        };

        const url = `api/Account/${accountNumber}/Deposit`;
        
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


        dispatch({ type: 'DEPOSIT_SUCCESS' });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: DepositState = { isLoading: false };

export const reducer: Reducer<DepositState> = (state: DepositState | undefined, incomingAction: Action): DepositState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'SUBMIT_DEPOSIT':
            return {
                isLoading: true
            };
        case 'DEPOSIT_SUCCESS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                isLoading: false,
            };
        default:
            return state;
    }
};
