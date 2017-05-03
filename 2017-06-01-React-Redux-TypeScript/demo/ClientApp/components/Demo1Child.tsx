import * as React from 'react';


export interface Props
{
    strings: string[];
}

export class ChildComponent extends React.Component<Props, void>
{
    public render()
    {
        let strings = this.props.strings;
        let listOfStrings = strings.map((value) => (<li>{value}</li>));

        return (<div><ul>{listOfStrings}</ul></div>);
    }
}