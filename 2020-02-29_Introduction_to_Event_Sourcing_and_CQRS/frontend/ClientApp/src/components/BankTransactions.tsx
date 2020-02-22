import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as BankTransactionsStore from '../store/BankTransactions';

// At runtime, Redux will merge together...
type BankTransactionProps =
  BankTransactionsStore.BankTransactionsState // ... state we've requested from the Redux store
  & typeof BankTransactionsStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ /*startDateIndex: string*/ }>; // ... plus incoming routing parameters


class BankTransactions extends React.PureComponent<BankTransactionProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }


  public render() {
    return (
      <React.Fragment>
        <div>
          <h1>Current Transactions</h1>
          <button disabled={this.props.isLoading} onClick={this.props.requestBankTransactions}>Refresh</button>

          <h2>Current Balance: {this.props.currentBalance}</h2>
          {this.renderTransactionsTable()}
        </div>
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestBankTransactions();
  }

  private renderTransactionsTable() {
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
          {this.renderMapTransactions(this.props.transactions)}
        </tbody>
      </table>
    );
  }

  private renderMapTransactions(txs: BankTransactionsStore.BankTransaction[]) {
    if(!txs) return [];

    return txs.map(tx => this.renderTransactionRow(tx));
  }

  private renderTransactionRow(tx: BankTransactionsStore.BankTransaction) {
    return (<tr key={tx.id}>
              <td>{tx.dateFormatted}</td>
              <td>{tx.type}</td>
              <td>{tx.amount}</td>
            </tr>);
  }
  
}

export default connect(
  (state: ApplicationState) => state.bankTransactions, // Selects which state properties are merged into the component's props
  BankTransactionsStore.actionCreators // Selects which action creators are merged into the component's props
)(BankTransactions as any);
