import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

const accountNumber = "4ff8fae5-e2fe-4d65-9f59-cf95cb5f31ea";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface BankTransactionsState {
    isLoading: boolean;
    transactions: BankTransaction[];
    currentBalance: number;
}

export interface BankTransaction {
    id: string;
    dateFormatted: string;
    type: string;
    amount: number;
    summary: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestBankTransactionsAction {
    type: 'REQUEST_BANK_TRANSACTIONS';
}

interface RequestBankTransactionsSuccessAction {
    type: 'BANK_TRANSACTIONS_SUCCESS';
    transactions: BankTransaction[];
    currentBalance: number;
}

interface RequestBankTransactionsErrorAction {
    type: 'BANK_TRANSACTIONS_ERROR';
}
// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = 
    RequestBankTransactionsAction | 
    RequestBankTransactionsSuccessAction | 
    RequestBankTransactionsErrorAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestBankTransactions: (): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();

        if (appState && appState.bankTransactions && appState.bankTransactions.isLoading) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }

        dispatch({ type: 'REQUEST_BANK_TRANSACTIONS' });

        const url = `api/Account/${accountNumber}/Transactions`;

        try
        {
            const response = await fetch(url, {
                method: "GET", // *GET, POST, PUT, DELETE, etc.
                mode: "cors", // no-cors, cors, *same-origin
                cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
                credentials: "same-origin", // include, *same-origin, omit
                headers: {
                    "Content-Type": "application/json",
                }
            });

            const data = await response.json();
            const transactions = data.transactions;
            const currentBalance = data.currentBalance;

            dispatch({ type: 'BANK_TRANSACTIONS_SUCCESS', transactions, currentBalance });
        } 
        catch(ex)
        {
            dispatch({ type: 'BANK_TRANSACTIONS_ERROR' });
        }
        
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: BankTransactionsState = { 
    isLoading: false,
    currentBalance: 0,
    transactions: [],
};

export const reducer: Reducer<BankTransactionsState> = (state: BankTransactionsState | undefined, incomingAction: Action): BankTransactionsState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_BANK_TRANSACTIONS':
            return {
                ...state,
                isLoading: true
            };
        case 'BANK_TRANSACTIONS_SUCCESS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                isLoading: false,
                currentBalance: action.currentBalance,
                transactions: action.transactions,
            };
        case 'BANK_TRANSACTIONS_ERROR':
            return {
                isLoading: false,
                currentBalance: 0,
                transactions: [],
            };
        default:
            return state;
    }
};
