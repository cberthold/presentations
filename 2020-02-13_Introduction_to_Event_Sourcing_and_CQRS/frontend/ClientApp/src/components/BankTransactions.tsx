import * as React from 'react';
import { connect } from 'react-redux';

const BankTransactions = () => (
  <div>
    <h1>Bank Transactions</h1>
  </div>
);

export default connect()(BankTransactions);
