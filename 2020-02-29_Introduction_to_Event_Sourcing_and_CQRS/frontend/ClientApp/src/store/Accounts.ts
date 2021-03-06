import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import { appendFileSync } from 'fs';


// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AccountState {
    isLoading: boolean;
    accounts: AccountModel[];
    selectedAccountId?: string | undefined;
}

export interface AccountModel {
    accountId: string;
    accountName: string;
    currentBalance: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface SubmitOpenNewAccountAction {
    type: 'SUBMIT_OPEN_NEW_ACCOUNT';
    firstName: string;
    lastName: string;
    accountType: string;
}

interface SubmitOpenNewAccountSuccessAction {
    type: 'OPEN_NEW_ACCOUNT_SUCCESS';
    firstName: string;
    lastName: string;
    accountType: string;
    accountId: string;
}

interface GetAccountsAction {
    type: 'SUBMIT_GET_ACCOUNTS';
}


interface GetAccountsSuccessAction {
    type: 'GET_ACCOUNTS_SUCCESS';
    accounts: AccountModel[];
}

interface SelectAccountAction {
    type: 'SELECT_ACCOUNT';
    accountId: string;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = 
    SubmitOpenNewAccountAction | 
    SubmitOpenNewAccountSuccessAction |
    GetAccountsAction |
    GetAccountsSuccessAction |
    SelectAccountAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    selectAccount: (accountId: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: 'SELECT_ACCOUNT', accountId });
    },
    getAccounts: (refresh: boolean): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();

        if (appState && 
            appState.account && 
            appState.account.accounts &&
            appState.account.accounts.length > 0 &&
            !refresh) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }

        const url = `api/Account`;
        
        try
        {
            
            dispatch({ 
                type: 'SUBMIT_GET_ACCOUNTS'
            });

            const response = await fetch(url, {
                method: "GET", // *GET, POST, PUT, DELETE, etc.
                mode: "cors", // no-cors, cors, *same-origin
                cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
                credentials: "same-origin", // include, *same-origin, omit
                headers: {
                    "Content-Type": "application/json",
                }
            });

            const responseJson = await response.json();
            
            dispatch({ 
                type: 'GET_ACCOUNTS_SUCCESS',
                accounts: responseJson,
            });
        } 
        catch(err)
        {
            console.log(err);
        }

    },
    submitOpenNewAccount: (
        firstName: string,
        lastName: string,
        accountType: string): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();

        if (appState && appState.account && appState.account.isLoading) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }

        const data = {
            firstName: firstName,
            lastName: lastName,
            accountType: accountType,
        };

        const url = `api/Account/Open`;
        
        try
        {
            
            dispatch({ 
                type: 'SUBMIT_OPEN_NEW_ACCOUNT',
                ...data,
            });

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

            const responseJson = await response.json();
            
            dispatch({ 
                type: 'OPEN_NEW_ACCOUNT_SUCCESS',
                ...data,
                accountId: responseJson,
            });
        } 
        catch(err)
        {
            console.log(err);
        }


    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: AccountState = { isLoading: false, accounts: [], selectedAccountId: undefined };

export const reducer: Reducer<AccountState> = (state: AccountState | undefined, incomingAction: Action): AccountState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'SUBMIT_OPEN_NEW_ACCOUNT':
            return {
                ...state,
                isLoading: true
            };
        case 'OPEN_NEW_ACCOUNT_SUCCESS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                ...state,
                isLoading: false,
            };
        case 'GET_ACCOUNTS_SUCCESS':
            let selectedAccountId = state.selectedAccountId;
            if(!selectedAccountId && action.accounts && action.accounts.length > 0)
            {
                selectedAccountId = action.accounts[0].accountId;
            }
            return {
                ...state,
                accounts: action.accounts,
                selectedAccountId: selectedAccountId,
            };
        case 'SELECT_ACCOUNT':
            return {
                ...state,
                selectedAccountId: action.accountId,
            };
        default:
            return state;
    }
};
