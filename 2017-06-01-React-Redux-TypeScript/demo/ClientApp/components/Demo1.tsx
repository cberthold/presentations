import * as React from 'react';
import * as Demo1Child from './Demo1Child';


export default class Demo1 extends React.Component<void,Demo1Child.Props>
{
    constructor()
    {
        super();
        let stringsArray: string[] = ['string1','string2','string3','string4'];
        this.state = { strings: stringsArray };
        
    }

    demoClicked()
    {
        let inputStateStrings = this.state.strings;
        let length = inputStateStrings.length;
        let newString = 'string' + ++length;
        let changeStateStrings = inputStateStrings.concat([newString]);
        let newState = Object.assign({}, this.state, { strings: changeStateStrings});
        this.setState(newState);
    }

    public render()
    {
        return (
        <div>
            <h1 onClick={this.demoClicked.bind(this)}>Demo</h1>
            <Demo1Child.ChildComponent strings={this.state.strings} />
        </div>);
    }
    

}