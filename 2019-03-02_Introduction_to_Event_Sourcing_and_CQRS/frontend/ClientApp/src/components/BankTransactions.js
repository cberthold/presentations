import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/BankTransactions';

class BankTransactions extends Component {
    componentWillMount() {
      // This method runs when the component is first added to the page
      this.props.requestBankTransactions();
    }
  
    componentWillReceiveProps(nextProps) {
      // This method runs when incoming props (e.g., route params) change
      this.props.requestBankTransactions();
    }
  
    render() {
      return (
          <div>
        <h1>Current Transactions</h1>
        <button disabled={props.isLoading} onClick={props.requestBankTransactions()}>Refresh</button>

        <h2>Current Balance: {props.currentBalance}</h2>

        {renderTransactionsTable(this.props)}
    </div>);
    }
  }
  
  function renderTransactionsTable(props) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Date</th>
            <th>Transaction Type</th>
            <th>Amount</th>
            {/*<th>Summary</th>*/}
          </tr>
        </thead>
        <tbody>
          {props.transactions.map(tx =>
            <tr key={tx.id}>
              <td>{tx.dateFormatted}</td>
              <td>{tx.type}</td>
              <td>{tx.amount}</td>
              {/*<td>{forecast.summary}</td>*/}
            </tr>
          )}
        </tbody>
      </table>
    );
  }
  


export default connect(
  state => state.transactions,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(BankTransactions);
