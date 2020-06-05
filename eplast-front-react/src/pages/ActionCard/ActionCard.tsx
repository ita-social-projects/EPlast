import React from 'react';
import { Card } from 'antd';
import 'antd/dist/antd.css';

import { useHistory } from "react-router-dom";


const classes = require('./ActionCard.module.css');

interface CardProps {
    title: string;
    name: string;
    imgUrl?: string;
    userId?: string;
    id: string;
}

interface Props {
    item: CardProps;
}

const ActionCard = ({
    item: { title, id , name},
}: Props) => {

    const { Meta } = Card;
    const history = useHistory();
    
    return (
        <div>
            <Card
                hoverable
                className={classes.cardStyles}
                cover={<img alt="example" src="https://eplast.azurewebsites.net/images/Events/ActionLogo.png" />}
                onClick={()=> history.push(`/actions/events/${id}`)}
            >
                <Meta title={title || name} className={classes.titleText}/>
            </Card>
        </div>
    )
}
export default ActionCard;



