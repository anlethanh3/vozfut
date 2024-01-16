import { Container, ListGroup, Row, Col, ListGroupItem, Alert } from 'react-bootstrap';
import { RivalMatchProps, RivalScheduleProps } from '../slices/matchDetailSlice';
import { MatchProps } from '../slices/matchSlice';
import { teamColors } from './Rivals';
export default function RivalSchedule(props: { data: RivalScheduleProps, match: MatchProps }) {
    const { data, match } = props

    function Match(props: { list: RivalMatchProps[], teamCount: number, isHome: boolean, startIndex: number }): JSX.Element {
        let { list, isHome, teamCount, startIndex } = props
        let colors = teamColors(teamCount)
        let items: JSX.Element[] = list.map((item, index) =>
            <ListGroup key={`list-match-${isHome ? 'home' : 'away'}-${index}`} className={index === 0 ? '' : 'mt-2'} horizontal>
                <ListGroupItem className='col-3'>Match {index + 1 + startIndex}</ListGroupItem>
                <ListGroupItem className='col-3' variant={colors[item.home].color}>{colors[item.home].name}</ListGroupItem>
                <ListGroupItem className='col-3' variant={colors[item.away].color}>{colors[item.away].name}</ListGroupItem>
            </ListGroup>
        )
        return (<>{items}</>)
    }

    return (
        <Alert variant='success'>
            <Container>
                <Row>
                    <Col>
                        <h1>Home Matches</h1>
                        <Container>
                            <Row>
                                <Match list={data.homeMatches} isHome teamCount={match.teamCount} startIndex={0} />
                            </Row>
                        </Container>
                    </Col>
                    <Col>
                        <h1>Away Matches</h1>
                        <Container>
                            <Row>
                                <Match list={data.awayMatches} isHome={false} teamCount={match.teamCount} startIndex={data.homeMatches.length} />
                            </Row>
                        </Container>
                    </Col>
                </Row>
            </Container>
        </Alert>
    )
}