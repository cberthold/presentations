import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as BankTransactionsStore from '../store/BankTransactions';
import * as AccountsStore from '../store/Accounts';
import { Dispatch, bindActionCreators } from 'redux';
import { AccountSelector } from './AccountSelector';


interface IBankTransactionProps
{
  transaction: BankTransactionsStore.BankTransactionsState;
  account: AccountsStore.AccountState;
  transactionActions: typeof BankTransactionsStore.actionCreators,
  accountActions: typeof AccountsStore.actionCreators,
}

type BankTransactionProps =
  IBankTransactionProps
  & RouteComponentProps<{ /*startDateIndex: string*/ }>; // ... plus incoming routing parameters

class BankTransactions extends React.PureComponent<BankTransactionProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  private requestTransactions()
  {
    if(this.props.account.selectedAccountId)
    {
      this.props.transactionActions.requestBankTransactions(this.props.account.selectedAccountId);
    }
  }

  private replayTransactions()
  {
    this.props.transactionActions.replayTransactions();
  }

  private selectAccountAndRefresh = async (accountId:string) => {
    const { selectAccount } = this.props.accountActions;
    await selectAccount(accountId);
    this.requestTransactions();
  }

  public render() {

    const { accounts, selectedAccountId } = this.props.account;
    const { isLoading, currentBalance, transactions } = this.props.transaction;
    return (
      <React.Fragment>
        <div>
          <h1>Current Transactions</h1>
          <p>Account: <AccountSelector accounts={accounts} onChange={(accountId) => this.selectAccountAndRefresh(accountId)} value={selectedAccountId} /></p>
    
          <button disabled={isLoading} onClick={() => this.requestTransactions()}>Refresh</button>

          <button disabled={isLoading} onClick={() => this.replayTransactions()}>Replay</button>

          <h2>Current Balance: {currentBalance}</h2>
          {this.renderTransactionsTable(transactions)}
        </div>
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.accountActions.getAccounts(true);
    this.requestTransactions();
  }

  private renderTransactionsTable(transactions: BankTransactionsStore.BankTransaction[]) {
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
          {this.renderMapTransactions(transactions)}
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


const mapDispatchToProps = (dispatch: Dispatch) =>
  ({
    transactionActions: bindActionCreators(BankTransactionsStore.actionCreators, dispatch),
    accountActions: bindActionCreators(AccountsStore.actionCreators, dispatch),
  });

export default connect(
  (state: ApplicationState) => {
    const returnState = {
      transaction : state.bankTransactions,
      account : state.account,
    }
    return returnState;
  },// Selects which state properties are merged into the component's props
  (dispatch: Dispatch) => {
    const returnProps = mapDispatchToProps(dispatch);
    return returnProps;
  }
)(BankTransactions as any);
