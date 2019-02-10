const submitWithdrawalType = 'SUBMIT_WITHDRAWAL';
const submitSuccessType = 'WITHDRAWAL_SUCCESS';
const initialState = { isLoading: false };
const accountNumber = "4ff8fae5-e2fe-4d65-9f59-cf95cb5f31ea";

export const actionCreators = {
  submitWithdrawal: (withdrawalAmount) => async (dispatch, getState) => {    
    if (getState().isLoading) {
      // Don't issue a duplicate request (we already have or are loading the requested data)
      return;
    }

    dispatch({ type: submitWithdrawalType });

    const data = {
      amount: withdrawalAmount,
    };

    const url = `api/Account/${accountNumber}/Withdrawal`;
    
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

    const response = await response.json();

    dispatch({ type: submitSuccessType, transactions, currentBalance });
  }
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === submitWithdrawalType) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === submitSuccessType) {
    return {
      ...state,
      isLoading: false
    };
  }

  return state;
};
