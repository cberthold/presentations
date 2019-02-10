import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Withdrawal';

const Withdrawal = props => (
  <div>
    <h1>Withdrawal Amount</h1>

    <p>$</p>

    <button onClick={props.submitWithdrawal(0.00)}>Withdrawal</button>
  </div>
);

export default connect(
  state => state.withdrawals,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Withdrawal);
