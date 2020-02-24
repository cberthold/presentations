import * as React from 'react';
import { AccountModel } from '../store/Accounts';

interface AccountSelectorProps
{
    accounts: AccountModel[],
    value?: string | undefined,
    onChange?: (value: string) => void,
}

export class AccountSelector extends React.PureComponent<AccountSelectorProps, {}> {
  constructor(props: AccountSelectorProps)
  {
    super(props);
  }

  private selectionChanged = (e: React.FormEvent<HTMLSelectElement>) => {
    const { selectedIndex } = e.currentTarget;
    const { accounts, onChange } = this.props;
    const selectedAccount = accounts[selectedIndex];
    if(onChange == null)
    {
        return;
    }
    onChange(selectedAccount.accountId);
  }


  public render() {
    const { accounts, value} = this.props;
    let accountsArray = accounts || [];
    return (<select onChange={this.selectionChanged} value={value}>
        {accountsArray.map((account: AccountModel) =>
            <option key={account.accountId}>{account.accountName}</option>
          )}
    </select>);
  }
}