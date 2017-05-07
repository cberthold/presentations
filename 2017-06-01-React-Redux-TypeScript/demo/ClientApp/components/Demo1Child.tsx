import * as React from 'react';


export interface Props
{
    strings: string[];
}

export class ChildComponent extends React.Component<Props, void>
{
    private renderItem(value)
    {
        return (<li>{value}</li>);
    }

    public render()
    {
        const strings = this.props.strings;
        const listOfStrings = strings.map(this.renderItem);

        return (<div><ul>{listOfStrings}</ul></div>);
    }
}